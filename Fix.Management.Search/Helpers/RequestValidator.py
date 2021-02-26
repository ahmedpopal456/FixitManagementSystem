def request_parameter_validator(request_param) -> bool:
    budget = request_param.get('budget')
    if budget and not budget.isnumeric() and float(budget) < 0:
        return False
    return True
