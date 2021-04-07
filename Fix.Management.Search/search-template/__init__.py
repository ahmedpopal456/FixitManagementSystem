import logging
import os

import azure.functions as func
from helpers import my_constants as object_result
from helpers import request_validator as validate
from mediator import fix_search_mediator as mediator

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    cs = os.environ['FIXIT_MDM_DB_CS']
    request_param = req.params

    if not request_param.get('keywords'):
        return func.HttpResponse(object_result.NULL_KEYWORDS, status_code=400)

    if not validate.request_parameter_validator(request_param):
        return func.HttpResponse(object_result.INVALID_PARAM, status_code=400)

    fix_mediator = mediator.FixSearchMediator(cs, request_param)
    response = fix_mediator.search_fix_template()

    if response:
        return func.HttpResponse(response, status_code=200)
    else:
        return func.HttpResponse(
            object_result.EMPTY_RESULT + request_param.get('keywords'),
            status_code=200
        )


         