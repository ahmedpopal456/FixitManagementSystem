terraform {
  backend "azurerm" {}
  required_version = "=0.14.5"

  required_providers {
    azurerm = {
        source = "hashicorp/azurerm",
        version = "=2.30.0"
    }
  }
}