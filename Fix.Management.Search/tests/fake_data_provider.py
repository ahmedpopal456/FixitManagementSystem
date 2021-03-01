import pandas as pd
from helpers import search_helper as search

def get_mock_sql_data():
    # intialise data of lists. 
    data = {'Id':[
                "BA60C506-05AE-4D7F-A68C-48A60599BDF6",
                "327F43E3-44B5-45B4-9D0D-7AE2E3CEFE76", 
                "A8681A20-6B93-4D44-A727-84CCFD2C0171",
                "8E141EC9-E2B4-46C0-B7D9-97CA83846A0"
                ], 
            'TemplateName':[
                'Bathroom Renovation', 
                'Kitchen Renovation', 
                'Home renovation', 
                'Plumbing'
                ],
            'Category':[
                'Bathroom',
                'Basement', 
                'Kitchen', 
                'Plumbing'
                ],
            'Type':[
                'New', 
                'Modernize', 
                'Repair', 
                'Repair'
                ],
            'Tags':[
                "Bathroom, Sinks, Tiles, Bathrub",
                "Basement, Cellar, Ground Floor",
                "Kitchen, Countertop, Stove, Tiles",
                "Bathroom, Toilet, Pipe"
                ],
            'Description':[
                "A bathroom or washroom is a room, typically in a home or other residential building, that contains either a bathtub or a shower (or both). The inclusion of a wash basin is common. In some parts of the world, a toilet is typically included in the bathroom; in others, the toilet is typically given a dedicated room separate from the one allocated for personal hygiene activities.",
                "A basement or cellar is one or more floors of a building that are completely or partly below the ground floor. It generally is used as a utility space for a building, where such items as the boiler, water heater, breaker panel or fuse box, car park, and air-conditioning system are located",
                "A modern yet deceptively simple looking New Zealand kitchen design has taken out the top award for the world's best residential kitchen. Davinia Sutton, of Christchurch's Detail by Davinia Sutton Ltd, received the International Design of the Year Award at the 2019 Designer Kitchen & Bathroom Awards, held in the UK.",
                "Plumbing, Plumber, toilet, sink, pipe, bathroom"
                ],
            'SystemCostEstimate':[
                100.0,
                50.0,
                123.0,
                500.0
            ]}
    # Create DataFrame 
    df = pd.DataFrame(data=data)  
    return df

def get_empty_mock_sql_data():
    data = {'Id':[], 
            'TemplateName':[],
            'Category':[],
            'Type':[],
            'Tags':[],
            'Description':[],
            'SystemCostEstimate':[],
            'concatTemplates':[],
            'bm25-score':[]}
    # Empty dataframe
    df = pd.DataFrame(data=data)   
    return df
