resource "azurerm_resource_group" "main" {
  name     = "${var.organization_name}-${var.environment_name}-${var.service_abb}"
  location = var.location_name
}

// TODO: verify configurations and naming with Ahmed
resource "azurerm_storage_account" "api" {
  name                     = "${var.organization_name}${var.environment_name}${var.service_abb}api"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = var.location_name
  account_tier             = "Standard"
  account_replication_type = "LRS"
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
  name = "${var.organization_name}-${var.environment_name}-${var.service_abb}-api"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  app_service_plan_id        = azurerm_app_service_plan.main.id
  storage_account_name       = azurerm_storage_account.api.name
  storage_account_access_key = azurerm_storage_account.api.primary_access_key
}
