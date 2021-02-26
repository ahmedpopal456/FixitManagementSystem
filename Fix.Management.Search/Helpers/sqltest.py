import pyodbc
import logging
import pandas as pd

class SqlDatabase():
    def __init__(self, sql_database_cs: str, request_param):
        self.sql_database_cs = sql_database_cs
        self.request_param = request_param
        self.sql_query = f"SELECT * FROM [dbo].[View_Fix_Template] {self.set_sql_filter()}"

    def get_sql_db(self):
        try:
            self.conn = pyodbc.connect(self.sql_database_cs)
            self.cursor = self.conn.cursor()
        except pyodbc.Error as e:
            logging.error(e)
        return None

    def set_sql_filter(self):
        filter : str = None
        self.budget = self.request_param.get('budget')
        budget_sql = """[SystemCostEstimate] <= ? """
        self.last_accessed = self.request_param.get('accessed')
        accessed_sql = """DATEADD(SECOND, [LastAccessedTimestampUtc],'1970-01-01') BETWEEN DATEADD(DAY, -7, GETDATE()) AND GETDATE() """

        if not self.budget and not self.last_accessed:
            return filter
        if self.budget and self.last_accessed:
            filter = f"WHERE {budget_sql} AND {accessed_sql}"
            return filter
        if self.budget:
            filter = f"WHERE {budget_sql}"
            return filter
        if self.last_accessed:
            filter = f"WHERE {accessed_sql}"

        return filter

    def fetch_templates(self, request_param):
        if self.budget:
            df = pd.read_sql(self.sql_query, self.conn, params={self.budget})
        else:
            df = pd.read_sql(self.sql_query, self.conn)
        return df
