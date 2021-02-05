resource "azurerm_resource_group" "main" {
  name     = "${var.organization_name}-${var.environment_name}-${var.service_abb}"
  location = var.location_name
}

resource "azurerm_storage_account" "main" {
  name                     = "${var.organization_name}${var.environment_name}${var.service_abb}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_account" "app" {
  for_each                 = toset(var.function_apps)
  name                     = "${var.organization_name}${var.environment_name}${var.service_abb}${each.key}"
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

resource "azurerm_function_app" "main" {
  for_each                   = toset(var.function_apps)
  name                       = "${var.organization_name}-${var.environment_name}-${var.service_abb}-${each.key}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  app_service_plan_id        = azurerm_app_service_plan.main.id
  storage_account_name       = azurerm_storage_account.app[each.key].name
  storage_account_access_key = azurerm_storage_account.app[each.key].primary_access_key

  app_settings = {
    "AzureWebJobsStorage" : "UseDevelopmentStorage=false",
    "FUNCTIONS_WORKER_RUNTIME" : "dotnet",
    "FIXIT-FMS-DB-EP" : data.azurerm_cosmosdb_account.main.endpoint,
    "FIXIT-FMS-DB-KEY" : data.azurerm_cosmosdb_account.main.primary_key,
    "FIXIT-FMS-DB-NAME" : data.azurerm_cosmosdb_account.main.name,
    "FIXIT-FMS-DB-FIXTABLE" : azurerm_cosmosdb_table.main["fixes"].name,
    "FIXIT-FMS-DB-FIXPLANTABLENAME" : azurerm_cosmosdb_table.main["fixplans"].name,

    "FIXIT-FMS-QUEUE-EP" : "https://${azurerm_storage_account.main.name}.queue.core.windows.net/${azurerm_storage_queue.main.name}",
    "FIXIT-FMS-STORAGEACCOUNT-CS" : azurerm_storage_account.main.primary_connection_string,
    "FIXIT-FMS-STORAGEACCOUNT-KEY" : azurerm_storage_account.main.primary_access_key,
    "FIXIT-FMS-QUEUE-NAME" : azurerm_storage_queue.main.name
  }
}

resource "azurerm_cosmosdb_table" "main" {
  for_each            = var.cosmosdb_tables
  name                = each.value
  resource_group_name = data.azurerm_cosmosdb_account.main.resource_group_name
  account_name        = data.azurerm_cosmosdb_account.main.name
  throughput          = 400
}
