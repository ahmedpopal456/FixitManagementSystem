from . import fix_search_mediator_interface as fsmi
from helpers import search_helper as search
from helpers import sql
from types import MappingProxyType

class FixSearchMediator(fsmi.FixSearchMediatorInterface):
    """Connects to Azure SQL Database and fetches Fix templates.

    Attributes
    ----------
    sql_database_cs : str
        Connection string of the SQL database.
    request_param : MappingProxyType
        Http request parameters.

    Methods
    -------
    search_fix_templates():
        Fetch Fix templates in SQL database and filter 
        the Fix templates based on the Search Matching Algorithmn.
        Returns a list of Fix Templates as a result.
    """
    
    def __init__(self, sql_database_cs: str, request_param: MappingProxyType) -> None:
        """ Constructs all the necessary attributes for the mediator.

        Parameters
        ----------
            sql_database_cs : str
                Connection string of the SQL database.
            request_param : MappingProxyType
                Http request parameters.
        """
        if not sql_database_cs:
            raise ValueError("Expected a connection string for sql database")
        if not request_param or not isinstance(request_param, MappingProxyType):
            raise ValueError("Expected query parameters but received null")

        self.sql_database_cs = sql_database_cs
        self.request_param = request_param
        self._sql_database_client = sql.SqlDatabase(self.sql_database_cs, self.request_param)
        self._sql_database_client.connect_sql_db()

    def search_fix_template(self) -> str:
        """ 
        Fetch Fix templates in SQL database and filter 
        the Fix templates based on the Search Matching Algorithmn.
        Returns a list of Fix Templates as a result.

        Returns
        -------
            results: str
                The resulting Fix templates in json format as a string.
        """
        df = self._sql_database_client.fetch_templates(self.request_param)

        fixTemplates = search.TemplateSearchMatching(df)
        fixTemplates.get_results(self.request_param.get('keywords'))
        results:str = fixTemplates.print_results()
        
        return results
