import pyodbc
import logging
import pandas as pd
from types import MappingProxyType

class SqlDatabase():
    """Connects to Azure SQL Database and fetches Fix templates.

    Attributes
    ----------
    sql_database_cs : str
        Connection string of the SQL database.
    request_param : MappingProxyType
        Http request parameters.

    Methods
    -------
    connect_sql_db():
        Creates connection to sql database and the connection cursor.
    set_sql_filter():
        Set and return filter(s) to be used in the SQL query.
    fetch_templates(request_param: MappingProxyType) -> pandas.DataFrame:
        Fetch and returns a pandas dataframe of Fix templates.
    """

    def __init__(self, sql_database_cs: str, request_param: MappingProxyType) -> None:
        """ Constructs all the necessary attributes for the SQL object.

        Parameters
        ----------
            sql_database_cs : str
                Connection string of the SQL database.
            request_param : MappingProxyType
                Http request parameters.
        """
        self.sql_database_cs = sql_database_cs
        self.request_param = request_param

    def connect_sql_db(self) -> None:
        """ Creates a connection to sql database and the connection cursor."""
        try:
            self.conn = pyodbc.connect(self.sql_database_cs)
            self.cursor = self.conn.cursor()
        except pyodbc.Error as e:
            logging.error(e)


    def _set_sql_filter(self) -> str:
        """ Set and return filter(s) to be used in the SQL query.

        Returns
        -------
            filter: str
                SQL query of the filter to be set.
        """
        filter : str
        self.budget: str = self.request_param.get('budget')
        budget_sql: str = """[SystemCostEstimate] <= ? """
        self.last_accessed: str = self.request_param.get('accessed')
        accessed_sql: str = """DATEADD(SECOND, [LastAccessedTimestampUtc],'1970-01-01') BETWEEN DATEADD(DAY, -7, GETDATE()) AND GETDATE() """

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

    def fetch_templates(self, request_param: MappingProxyType) -> pd.DataFrame:
        """ Fetch and returns a pandas dataframe of Fix templates.

        Parameters
        ----------
            request_param: MappingProxyType
                Http request parameters.

        Returns
        -------
            df: pandas.DataFrame
                Fetch and returns a pandas dataframe of Fix templates.
        """
        sql_query = f"SELECT * FROM [dbo].[View_Fix_Template] {self._set_sql_filter()}"
        if self.budget:
            df = pd.read_sql(sql_query, self.conn, params={self.budget})
        else:
            df = pd.read_sql(sql_query, self.conn)
        return df
