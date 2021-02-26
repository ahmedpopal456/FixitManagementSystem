import unittest
import pandas as pd
import azure.functions as func
from Helpers import SearchHelper as search
from Mediator import FixSearchMediator as mediator
from Helpers import RequestValidator as validate
from Helpers import sqltest as sql
from . import fake_data_provider as mock

class TestFunction(unittest.TestCase):

    def test_search_matchinge_no_template_found(self):
        # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': '6U1S75'})
        
        request_param = req.params

        # Get mock data
        fake_sql_table = mock.get_mock_sql_data()

        # Call the function.
        fix_templates = search.SearchMatching(fake_sql_table)
        result = fix_templates.get_results(request_param.get('keywords'))

        # Check the output.
        self.assertEqual(result.empty, True)

    def test_search_matching_template_found(self):
        # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom'})
        
        request_param = req.params

        # Get mock data
        fake_sql_table = mock.get_mock_sql_data()

        # Call the function.
        fix_templates = search.SearchMatching(fake_sql_table)
        templates = fix_templates.get_results(request_param.get('keywords'))
        result = fix_templates.print_results()

        # Check the output.
        self.assertEqual(templates.empty, False)
        self.assertEqual(result is not None, True)

    def test_validate_query_param_return_false(self):
        # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom', 'budget': '-20'})
        
        request_param = req.params

        # Call the function.
        result = validate.request_parameter_validator(request_param)

        # Check the output.
        self.assertEqual(result, False)

    def test_validate_query_param_return_true(self):
        # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom', 'budget': '30'})
        
        request_param = req.params

        # Call the function.
        result = validate.request_parameter_validator(request_param)

        # Check the output.
        self.assertEqual(result, True)

    def test_sql_set_filter_none(self):
         # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom'})
        
        request_param = req.params
        cs = "anyCs"

        # Call the function.
        db = sql.SqlDatabase(cs, request_param)
        result = db.set_sql_filter()

        # Check the output.
        self.assertEqual(result, None)

    def test_sql_set_filter_budget(self):
         # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom', 'budget': '30'})
        
        request_param = req.params
        cs = "anyCs"
        budget_sql_filter = "WHERE [SystemCostEstimate] <= ?"

        # Call the function.
        db = sql.SqlDatabase(cs, request_param)
        result = db.set_sql_filter()

        # Check the output.
        self.assertEqual(result.strip(), budget_sql_filter.strip())

    def test_sql_set_filter_accessed(self):
         # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom', 'accessed': 'True'})
        
        request_param = req.params
        cs = "anyCs"
        accessed_sql_filter = """WHERE DATEADD(SECOND, [LastAccessedTimestampUtc],'1970-01-01') BETWEEN DATEADD(DAY, -7, GETDATE()) AND GETDATE()  """

        # Call the function.
        db = sql.SqlDatabase(cs, request_param)
        result = db.set_sql_filter()

        # Check the output.
        self.assertEqual(result.strip(), accessed_sql_filter.strip())

    def test_sql_set_filter_budget_and_accessed(self):
         # Construct a mock HTTP request.
        req = func.HttpRequest(
            method='GET',
            body=None,
            url='/api/SearchTemplates',
            params={'keywords': 'bathroom', 'budget': '30', 'accessed': 'True'})
        
        request_param = req.params
        cs = "anyCs"
        budget_sql_filter = "[SystemCostEstimate] <= ? "
        accessed_sql_filter = """DATEADD(SECOND, [LastAccessedTimestampUtc],'1970-01-01') BETWEEN DATEADD(DAY, -7, GETDATE()) AND GETDATE() """
        sql_filter =  f"WHERE {budget_sql_filter} AND {accessed_sql_filter}"
        # Call the function.
        db = sql.SqlDatabase(cs, request_param)
        result = db.set_sql_filter()

        # Check the output.
        self.assertEqual(result, sql_filter)
    
    
