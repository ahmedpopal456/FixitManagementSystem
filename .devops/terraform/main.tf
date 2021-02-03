resource "azurerm_resource_group" "main" {
  name = "${var.organization_name}-${var.environment_name}-${var.service_abb}"
  location = var.location_name
}

// TODO: verify configurations with Ahmed
resource "azurerm_storage_account" "api" {
  name = "test"
  resource_group_name = azurerm_resource_group.main.name
  location = var.location_name
  account_tier = "Standard"
  account_replication_type = "LRS"
}
