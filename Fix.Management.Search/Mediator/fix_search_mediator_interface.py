from abc import ABC

class FixSearchMediatorInterface(ABC):
    """Implements mediator interface for FixSearchMediator

    Methods
    -------
    search_fix_templates():
        Fetch Fix templates in SQL database and filter 
        the Fix templates based on the Search Matching Algorithmn.
        Returns a list of Fix Templates as a result.
    """
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
        pass
