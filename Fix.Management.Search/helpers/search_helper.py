import spacy
import pandas as pd
import logging
from rank_bm25 import BM25Okapi
# Load English tokenizer, tagger, parser, NER and word vectors
nlp = spacy.load("en_core_web_sm")

class TemplateSearchMatching():
    """
    Applies BM-25(TF-IDF) searching algorithm and system matching algorithm to get the desired list of Fix templates.

    Attributes
    ----------
    df : pandas.DataFrame
        Connection string of the SQL database.

    Methods
    -------
    tokenize_templates():
        Tokenize Fix templates and append to the DataFrame in order to use BM25.
    bm25(query: str):
        Calculates the score of Fix templates in order of relevance to a search query. 
    match():
        Calculates the final score of Fix templates using system defined features and filter out irrelevant Fix templates.
    get_results(query: str) -> pd.DataFrame:
        Get the resulting Fix templates after applying the search and matching algorithm.
    print_results() -> str:
        Prints the resulting Fix templates in json format as a string.

    """

    def __init__(self, df: pd.DataFrame):
        """ Constructs all the necessary attributes for the search matching algorithm.

        Attributes
        ----------
            df : pandas.DataFrame
                Connection string of the SQL database.
        """
        self.templates = df
        self.tok_templates = []

    def tokenize_templates(self) -> None:
        """ Tokenize Fix templates and append to the DataFrame in order to use BM25."""

        # Remove duplicates
        for col in ['Tags']:
            self.templates[col] = self.templates[col].str.split(", ").map(set).str.join(", ")

        # Concatenate all description field
        cols = ['TemplateName', 'WorkCategory', 'FixUnit', 'Tags', 'Description']
        self.templates['concatTemplates'] = self.templates[cols].apply(lambda row: ' '.join(row.values.astype(str)), axis=1)

        for doc in nlp.pipe(self.templates['concatTemplates'].str.lower().values, disable=["tagger", "parser", "ner"]):
            token = [(ent.text) for ent in doc]
            self.tok_templates.append(token)

    def bm25(self, query: str) -> None:
        """ Calculates the score of Fix templates by their relevance to a search query."""
        # Format search query
        tok_query = query.lower().strip('\"').split(" ") 
        # Apply BM25 (TF-IDF)
        try:
            bm_25 = BM25Okapi(self.tok_templates)
            results = bm_25.get_scores(tok_query)
            self.templates.insert(len(self.templates.columns),'bm25-score', results)
            self.templates['bm25-score'] = self.templates['bm25-score'].abs()
        except ZeroDivisionError:
            logging.exception("Unable to apply bm25. No Fix Templates to compare to.")
            self.templates['bm25-score'] = 0

    """
    TODO: Integrate match(), the second part of algorithm, once the system features are confirmed.
        We are missing data from system features that need to be implemented in the backend first.
        match() is currently a placeholder until the missing data are available.
    """
    def match(self) -> None:
        """ Calculates the final score of Fix templates using system defined features and filter out irrelevant Fix templates."""
        weighted_sum = []
        weighted_sum = self.templates["Template Frequency"].apply(lambda x: x*0.1) \
            + self.templates["Template Over Time"].apply(lambda x: x*0.15) \
            + self.templates["Validity"].apply(lambda x: x*0.05) \
            + self.templates["Template Rating"].apply(lambda x: x*0.25) \
            + self.templates["User Preference"].apply(lambda x: x*0.15) \
            + self.templates["bm25-score"].apply(lambda x: x*0.3) \

        self.templates.insert(len(self.templates.columns),
                              "final-score", weighted_sum)
        self.templates = self.templates[self.templates["final-score"]
                                        >= self.templates["final-score"].mean()]

    def get_results(self, query: str) -> pd.DataFrame:
        """ Get the resulting Fix templates after applying the search and matching algorithm.
        
        Returns
        -------
            templates: pandas.DataFrame
                DataFrame containing Fix templates 

        """
        self.tokenize_templates()
        self.bm25(query)
        """
        TODO: Integrate self.match(), the second part of algorithm, once the system features are confirmed.
            We are missing data from system features that need to be implemented in the backend first.
            self.match() is currently a placeholder until the missing data are available.
        """
        # self.match()

        # Filter out all templates that has score zero and sort values by descending order
        # TODO: Sort value by 'final-score' after second layer is implemented
        self.templates = self.templates[(self.templates['bm25-score'] != 0)].sort_values(by='bm25-score', ascending=False)
        
        return self.templates
    
    def print_results(self):
        """ Prints the resulting Fix templates in json format as a string.
        
        Returns
        -------
            str
                Returns the resulting Fix templates in json format as a string.

        """
        return self.templates[["Id", "TemplateName", "WorkCategory", "FixUnit", "Tags", "SystemCostEstimate"]].to_json(orient='records')
