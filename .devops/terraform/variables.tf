variable "subscription_name" {
  type = string
}

variable "subscription_id" {
  type = string
}

variable "environment_name" {
  type = string
}

variable "location_name" {
  type = string
}

variable "location_name_linux" {
  type = string
}

variable "organization_name" {
  type    = string
  default = "fixit"
}

variable "tenant_id" {
  type    = string
  default = "ccc68497-f4c0-4c2c-b499-78c30c54b52c"
}

variable "service_abbreviation" {
  type    = string
  default = "fms"
}

variable "function_apps" {
  type = list(string)
  default = [
    "api",
    "trigger"
  ]
}

variable "cosmosdb_tables" {
  type = map(string)
  default = {
    fixes = "Fixes",
    fixplans = "FixPlans",
    fixlocation = "FixLocation",
    fixtags = "FixTags"
  }
}

/* This value needs to be defined in the respective environment's tfvars file. The value must be defined manually 
for any new environment to reduce any dependencies on other services for the deployment. */
variable "cms_connection_string" {
  type = string
}