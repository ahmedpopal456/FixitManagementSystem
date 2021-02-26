import spacy
import pandas as pd
from rank_bm25 import BM25Okapi
# Load English tokenizer, tagger, parser, NER and word vectors
nlp = spacy.load("en_core_web_sm")

class SearchMatching():

    def __init__(self, df: pd.DataFrame):
        self.templates = df
        self.tok_templates = []

    def tokenize_templates(self):
        # Concatenate all description field
        self.templates['concatTemplates'] = self.templates['TemplateName'] + " " + \
                                            self.templates['Category'] + " " + \
                                            self.templates['Type'] + " " + \
                                            self.templates['Tags'] + " " + \
                                            self.templates['Description']

        for doc in nlp.pipe(self.templates['concatTemplates'].str.lower().values, disable=["tagger", "parser", "ner"]):
            token = [(ent.text) for ent in doc]
            self.tok_templates.append(token)

    def bm25(self, query):
        # Format search query
        tok_query = query.lower().strip('\"').split(" ") 
        # Apply BM25 (TF-IDF)
        bm_25 = BM25Okapi(self.tok_templates)
        results = bm_25.get_scores(tok_query)
        self.templates.insert(len(self.templates.columns),
                              "bm25-score", results)

    # TODO: Implement second part of algorithm once the system features are confirmed
    # Currently not used
    def match(self):
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

    def get_results(self, query):
        self.tokenize_templates()
        self.bm25(query)
        # TODO: Implement second part of algorithm once the system features are confirmed
        # self.match()

        # Filter out all templates that has score zero and sort values by descending
        # TODO: Sort value by final-score after second layer is implemented
        self.templates = self.templates[(self.templates != 0).all(1)].sort_values(by='bm25-score', ascending=False)
        return self.templates
    
    def print_results(self):
        return self.templates[["Id", "TemplateName", "Category", "Type", "Tags", "SystemCostEstimate"]].to_json(orient='records')

