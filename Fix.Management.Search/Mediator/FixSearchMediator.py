import os
from abc import ABC
from Helpers import SearchHelper as search
from Helpers import sqltest

# Interface
class FixSearchMediatorInterface(ABC):
    def search_fix_template(self) -> str:
        pass

class FixSearchMediator(FixSearchMediatorInterface):
    
    def __init__(self, sql_database_cs : str, request_param) -> None:
        if not sql_database_cs:
            raise ValueError("Expected a connection string for sql database")
        if not request_param:
            raise ValueError("Expected query parameters but received null")

        self.sql_database_cs = sql_database_cs
        self.request_param = request_param
        self.sqlDb = sqltest.SqlDatabase(self.sql_database_cs, self.request_param)
        self.sqlDb.get_sql_db()
        

    def search_fix_template(self):
        df = self.sqlDb.fetch_templates(self.request_param)
        if df.empty:
            return "No templates returned"

        fixTemplates = search.SearchMatching(df)
        fixTemplates.get_results(self.request_param.get('keywords'))
        results:str = fixTemplates.print_results()
        
        return results
