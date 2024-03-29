# elk-workshop-de
Eine kurze Einführung in den **Elastic Stack** und Nutzung vom diesem für ein zentralisiertes und strukturiertes Logging in Microservice-Architekturen.
Die Implementierung des Loggings basiert dabei auf **Microsoft.Logging.Abstractions** und **Serilog**.
Zudem wird das Theme der Überwachung von Services und entsprechende Integritiätsprüfungen behandelt.
Die Beispielanwendung verwendet dazu eine Implementierung basierend auf der Health Check Middleware von ASP.NET Corre.

Dieses Repository enthält den Code für die Beispielanwendung und ist als eine Art Workshop gedacht um sich mit den genannten Technologien vetraut zu machen und erste eigene Implementierungen vornehmen zu können. Der Workshop mit Details zur Beispielanwendung und Erklärung der Implementation sind über das [Wiki](https://github.com/Wiesenwischer/elk-workshop-de/wiki) verfügbar.

## Build Status (GitHub Actions)

| Image | Status | Image | Status |
| ------------- | ------------- | ------------- | ------------- |
| Health Monitor |  [![itops-healthmonitor](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/itops-healthmonitor.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/itops-healthmonitor.yml) | Elasticsearch | [![elasticsearch](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/elasticsearch.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/elasticsearch.yml) |
| Logstash | [![logstash](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/logstash.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/logstash.yml) | Kibana |[![kibana](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/kibana.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/kibana.yml) |
| Content Management Endpoint |[![store-contentmanagement](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-contentmanagement.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-contentmanagement.yml) | Customer Relations Endpoint | [![store-customerrelations](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-customerrelations.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-customerrelations.yml) |
| Operations Endpoint | [![store-operations](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-operations.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-operations.yml) | Sales Endpoint | [![store-sales](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-sales.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-sales.yml) |
| ECommerce (MVC) | [![store-ecommerce](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-ecommerce.yml/badge.svg)](https://github.com/Wiesenwischer/elk-workshop-de/actions/workflows/store-ecommerce.yml) | |  |

_**Main** branch enthält den atuellen Entwicklungsstand und die entsprechenden Images sind getagged mit `:latest` auf [Docker Hub](https://hub.docker.com/u/maddev77)_
