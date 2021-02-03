resource "azurerm_resource_group" "main" {
  name     = "${var.organization_name}-${var.environment_name}-${var.service_abb}"
  location = var.location_name
}

// TODO: verify configurations and naming with Ahmed
resource "azurerm_storage_account" "api" {
  name                     = "${var.organization_name}${var.environment_name}${var.service_abb}api"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_queue" "main" {
  name                 = "${var.organization_name}-${var.environment_name}-${var.service_abb}-queue"
  storage_account_name = azurerm_storage_account.main.name
}

resource "azurerm_app_service_plan" "main" {
  name                = "${var.organization_name}-${var.environment_name}-${var.service_abb}-service-plan"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_function_app" "api" {
  name                       = "${var.organization_name}-${var.environment_name}-${var.service_abb}-api"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  app_service_plan_id        = azurerm_app_service_plan.main.id
  storage_account_name       = azurerm_storage_account.api.name
  storage_account_access_key = azurerm_storage_account.api.primary_access_key

  // TEMPORARY CONFIG
  app_settings = {
    "IsEncrypted" : false,
    "Values" : {
      "AzureWebJobsStorage" : "UseDevelopmentStorage=true",
      "FUNCTIONS_WORKER_RUNTIME" : "dotnet",
      "FIXIT-FMS-DB-EP" : "https://jlincosmostest.documents.azure.com:443/",
      "FIXIT-FMS-DB-KEY" : "ETlBZgyZ443pHkiywqOnMJ6OUFAtLhVCr3IxhCkVptcVcCHP0JXpglFEHqnp5drnj5UCQSUuhPZkhOgtwiIGUA==",
      "FIXIT-FMS-DB-NAME" : "cosmostest",
      "FIXIT-FMS-DB-FIXTABLE" : "Fixes",
      "FIXIT-FMS-DB-FIXPLANTABLENAME" : "FixPlans",

      "FIXIT-FMS-QUEUE-EP" : "https://stchend.queue.core.windows.net/queuetest",
      "FIXIT-FMS-QUEUE-CS" : "DefaultEndpointsProtocol=https;AccountName=stchend;AccountKey=RWzJev5oocpzEqVeg4Ap2IKyxOBTgoMJw5ULVn1cFn+xDfjkZjSLOKScgRXwNK4otFMnunXKg0Pwmm6xlgFgMA==;EndpointSuffix=core.windows.net",
      "FIXIT-FMS-QUEUE-KEY" : "RWzJev5oocpzEqVeg4Ap2IKyxOBTgoMJw5ULVn1cFn+xDfjkZjSLOKScgRXwNK4otFMnunXKg0Pwmm6xlgFgMA==",
      "FIXIT-FMS-QUEUE-NAME" : azurerm_storage_queue.main.name
    }
  }
}
