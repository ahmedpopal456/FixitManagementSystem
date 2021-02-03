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

variable "organization_name" {
  type = string
  default = "fixit"
}

variable "tenant_id" {
  type = string
  default = "ccc68497-f4c0-4c2c-b499-78c30c54b52c"
}

variable "service_abb" {
  type = string
  default = "fms"
}