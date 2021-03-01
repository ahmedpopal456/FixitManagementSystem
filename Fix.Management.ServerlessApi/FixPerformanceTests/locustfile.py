from locust import HttpUser, task, between, tag
import uuid

class FixitFix(HttpUser):
    host = "http://localhost:7071"
    wait_time = between(2, 10)


    @tag('update_fix', 'fix')
    @task(2)
    def update_fix(self):
        self.client.put("/api/fixes/3e7b6395-31e3-465c-876b-1b9b8742c91a",
        json={
            "Details": [
                {
                    "Name": "Kitchen Sink Repair22",
                    "Description": "Repair kitchen Sink",
                    "Category": "Kitchen",
                    "Type": "Repair"
                }
            ],
            "Tags": [
                {
                    "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea98",
                    "Name": "Bedroom2"
                },
                {
                    "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea99",
                    "Name": "Sleeping Room2"
                }
            ],
            "Images": [
                {
                    "Name": "Bedroom Image",
                    "Url" : "/image-Bedroom.png"
                }
            ],
            "Location": {
                "Address": "3334 rue Jean-Talon2",
                "City": "Montreal2",
                "Province": "Quebec2",
                "PostalCode": "H4S 2D2"
            },
            "Schedule": [
                {
                    "StartTimestampUtc": 1609102412,
                    "EndTimestampUtc": 1609102532
                }
            ],
            "UpdatedByUser": {
                "Id": "c068fbd8-1c6e-4e78-b51b-2f52048e0518",
                "FirstName": "Mary",
                "LastName": "Sue"
            }
        },
        name="update_fix")


    @tag('update_fix_assign_request', 'fix')
    @task(2)
    def update_fix_assign_request(self):
        self.client.put("/api/fixes/3e7b6395-31e3-465c-876b-1b9b8742c91a/users/61735fb5-b3dd-48c0-bff2-919c267d11da/assign",
        json={
            "assignedToCraftsman": {
                "Id": "61735fb5-b3dd-48c0-bff2-919c267d11da",
                "FirstName": "John",
                "LastName": "Doe"
            },
            "ClientEstimatedCost": {
                "MaximumCost": 9000,
                "MinimumCost": 200
            },
            "systemCalculatedCost": 1235,
            "CraftsmanEstimatedCost": {
                "cost": 665,
                "comment": "take it or leave"
            },
            "UpdatedByUser": {
                "Id": "c068fbd8-1c6e-4e78-b51b-2f52048e0518",
                "FirstName": "John",
                "LastName": "Doe"
            }
        },
        name="update_fix_assign_request")
    
    @tag('get_fix_request', 'fix')
    @task(5)
    def get_fix_request(self):
        self.client.get("/api/fixes/3e7b6395-31e3-465c-876b-1b9b8742c91a",
        name="get_fix_request")

    @tag('get_fix_cost_request', 'fix')
    @task(5)
    def get_fix_cost_request(self):
        self.client.get("/api/fixes/3e7b6395-31e3-465c-876b-1b9b8742c91a/cost",
        name="get_fix_cost_request")


    @tag('create_fix', 'fix')
    @task(1)
    def create_fix(self):
        self.client.post("/api/fixes",
            json={
                "Details": [
                    {
                        "Name": "Bedroom",
                        "Description": "Bedroom needs to be revamped",
                        "Category": "Bedroom",
                        "Type": "New"
                    }
                ],
                "Tags":[
                    {
                        "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea98",
                        "Name": "Bedroom"
                    },
                    {
                        "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea99",
                        "Name": "Sleeping Room"
                    }
                ],
                "Images": [
                    {
                        "Name": "Bedroom Image",
                        "Url" : "/image-Bedroom.png"
                    }
                ],
                "Location": {
                    "Address": "334 rue Jean-Talon2",
                    "City": "Montreal",
                    "Province": "Quebec",
                    "PostalCode": "H4S 2D2"
                },
                "Schedule": [
                    {
                        "StartTimestampUtc": 1609102412,
                        "EndTimestampUtc": 1609102532
                    }
                ],
                "CreatedByClient": {
                    "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea93",
                    "FirstName": "Mike",
                    "LastName": "Kunk"
                },
                "UpdatedByUser": {
                    "Id": "8b418766-4a99-42a8-b6d7-9fe52b88ea93",
                    "FirstName": "Mike",
                    "LastName": "Kunk"
                }
            },
            name="create_fix")
