import logging
import os

import azure.functions as func
from Helpers import RequestValidator as validate
from Mediator import FixSearchMediator as mediator

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    cs = os.environ['FIXIT-SQL-CS']
    request_param = req.params

    if not request_param.get('keywords'):
        return func.HttpResponse(
            "Please pass keywords on the query string",
            status_code=200
        )
    if not validate.request_parameter_validator(request_param):
        return func.HttpResponse(
            f"Invalid Request. One or many request parameters have invalid fields",
            status_code=400
        )

    fix_mediator = mediator.FixSearchMediator(cs, request_param)
    response = fix_mediator.search_fix_template()

    if response:
        return func.HttpResponse(response)
    else:
        return func.HttpResponse(
            f"No templates found for {request_param.get('keywords')}",
             status_code=501
        )


         