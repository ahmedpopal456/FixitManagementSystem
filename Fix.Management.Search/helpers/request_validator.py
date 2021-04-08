def request_parameter_validator(request_param) -> bool:
    """ Validates Http request parameters

        Parameters
        ----------
        request_param : MappingProxyType
            Http request parameters.
            
        Returns
        -------
        bool
            True -- Request parameters are valid
            False -- Request parameters are not valid
    """
    budget = request_param.get('budget')
    if budget and not budget.isnumeric() and float(budget) < 0:
        return False
    return True
