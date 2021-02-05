# data "azurerm_key_vault" "main" {
#     name = ""
#     resource_group_name = azurerm_resource_group.main.name
# }

data "azurerm_cosmosdb_account" "main" {
  name                = "${var.organization_name}-${var.environment_name}-${var.service_abb}-cosmosdb"
  resource_group_name = azurerm_resource_group.main.name
}