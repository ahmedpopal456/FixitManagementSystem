from locust import HttpUser, task, between, tag
import uuid

class FixitFix(HttpUser):
    host = "http://localhost:7071"
    wait_time = between(2, 10)
    fix_id = ""
    DESCRIPTION = "Task Description"


    @tag('get_fix_plan', 'fix')
    @task(5)
    def get_fix_plan(self):
        self.client.get(f"http://localhost:7071/api/fixPlans/{self.fix_id}",
        name="get_fix_plan")

    @tag('get_fix_plan_history', 'fix')
    @task(5)
    def get_fix_plan_history(self):
        self.client.get(f"http://localhost:7071/api/fixPlans/users/{self.fix_id}/history",
        name="get_fix_plan_history") 

    @tag('update_fix_plan_crafsman', 'fix')
    @task(2)
    def update_fix_plan_crafsman(self):
        self.client.put(f"http://localhost:7071/api/fixPlans/{self.fix_id}/structure",
        json={
            "phases": [
                {
                    "id": "44c67585-a234-4bb2-852c-eccd17293058",
                    "name": "hello",
                    "status": 0,
                    "tasks": [
                        {
                            "id": "f650e970-084a-4a9e-bce9-630c2589f346",
                            "name": "no",
                            "description": "Task",
                            "status": 0
                        }
                    ]
                }
            ]
        },
        name="update_fix_plan_crafsman")

    @tag('update_fix_phase_crafsman', 'fix')
    @task(2)
    def update_fix_phase_crafsman(self):
        self.client.put(f"http://localhost:7071/api/fixPlans/{self.fix_id}/phases/44c67585-a234-4bb2-852c-eccd17293058/status",
        json={
            "id": "44c67585-a234-4bb2-852c-eccd17293058",
            "name": "Start",
            "status": 3,
            "tasks": [
                {
                    "id": "f650e970-084a-4a9e-bce9-630c2589f346",
                    "name": "name",
                    "description": DESCRIPTION,
                    "order": 0,
                    "status": 0
                },
                {
                    "id": "f770e970-084a-4a9e-bce9-630c2589f346",
                    "name": "name",
                    "description": DESCRIPTION,
                    "order": 0,
                    "status": 0
                }
            ]
        },
        name="update_fix_phase_crafsman")

    @tag('update_fix_phase_task_crafsman', 'fix')
    @task(2)
    def update_fix_phase_task_crafsman(self):
        self.client.put(f"http://localhost:7071/api/fixPlans/{self.fix_id}/phases/44c67585-a234-4bb2-852c-eccd17293058/tasks/f650e970-084a-4a9e-bce9-630c2589f346/status",
        json={
            "name": "testing",
            "description": DESCRIPTION,
            "order": 0,
            "status": 0
        },
        name="update_fix_phase_task_crafsman")

    @tag('update_state_client', 'fix')
    @task(2)
    def update_state_client(self):
        self.client.put(f"http://localhost:7071/api/fixPlans/{self.fix_id}/approve",
        json={
            "fixId": "35dec73a-f7c6-4f26-b96a-de3fa980950a",
            "activePhaseID": "f1aa52f2-bfe7-4435-ac99-0a0ef4e9baa1",
            "proposalState": 1,
            "billingDetails": {
                "initialCost": "100",
                "phaseCosts": [
                    {
                        "phaseEstimatedCost": "25"
                    }
                ],
                "endCost": "200"
            },
            "phases": [
                {
                    "id": "44c67585-a234-4bb2-852c-eccd17293058",
                    "name": "Start",
                    "tasks": {
                        "id": "f650e970-084a-4a9e-bce9-630c2589f346",
                        "name": "test",
                        "description": DESCRIPTION,
                        "order": "1"
                    }
                }
            ],
            "createdByCraftsman": {
                "id": "93432fb0-7589-4533-8908-3aa4b19e0734",
                "firstName": "John",
                "lastName": "Doe"
            },
            "totalCost": "5.0"
        },
        name="update_state_client")

    @tag('update_phase_client', 'fix')
    @task(2)
    def update_phase_client(self):
        self.client.put(f"http://localhost:7071/api/fixPlans/{self.fix_id}/phases/44c67585-a234-4bb2-852c-eccd17293058/approve",
        json={
            "id": "44c67585-a234-4bb2-852c-eccd17293058",
            "name": "Hello",
            "status": 1
        },
        name="update_phase_client")

    @tag('create_fix_plan', 'fix')
    @task(1)
    def create_fix_plan(self):
        response = self.client.post("/api/fixPlans",
            json={
                "fixId": "35dec73a-f7c6-4f26-b96a-de3fa980950a",
                "activePhaseID": "f1aa52f2-bfe7-4435-ac99-0a0ef4e9baa1",
                "proposalState": 0,
                "billingDetails": {
                    "initialCost": "100",
                    "phaseCosts": [
                        {
                            "phaseEstimatedCost": "25"
                        }
                    ],
                    "endCost": "200"
                },
                "phases": [
                    {
                        "id": "44c67585-a234-4bb2-852c-eccd17293058",
                        "name": "Start",
                        "tasks": [
                            {
                                "id": "f650e970-084a-4a9e-bce9-630c2589f346",
                                "name": "testing",
                                "description": "Task Description"
                            },
                            {
                                "id": "f770e970-084a-4a9e-bce9-630c2589f346",
                                "name": "testing2",
                                "description": "Task Description 2"
                            }
                        ]
                    }
                ],
                "createdByCraftsman": {
                    "id": "93432fb0-7589-4533-8908-3aa4b19e0734",
                    "firstName": "John",
                    "lastName": "Doe"
                },
                "totalCost": "10.0"
            },
            name="create_fix_plan")
        json_response = response.json()
        self.fix_id = json_response['id']

    def on_start(self):
        self.create_fix_plan()