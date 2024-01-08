# beangotown-server
BRANCH | AZURE PIPELINES                                                                                                                                                                                                             | TESTS                                                                                                                                                                                                 | CODE COVERAGE
-------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------
MASTER   | [![Build Status](https://dev.azure.com/BeangoTown/beangotown-server/_apis/build/status/beangotown-server?branchName=master)](https://dev.azure.com/BeangoTown/beangotown-server/_build/latest?definitionId=1&branchName=master) | [![Test Status](https://img.shields.io/azure-devops/tests/BeangoTown/beangotown-server/1/master)](https://dev.azure.com/BeangoTown/beangotown-indexer/_build/latest?definitionId=2&branchName=master) | [![codecov](https://codecov.io/github/Beangotown/beangotown-server/graph/badge.svg?token=ORWZI8I1SH)](https://codecov.io/github/Beangotown/beangotown-server)
## Introduction
BeangoTown Server provides interface services for the BeangoTown Game. In terms of project architecture, the project is developed based on the ABP framework.  In terms of data storage, the project uses  Elasticsearch for data storage and retrieval.  In summary, BeangoTown Server combines the advantages of the ABP framework and Elasticsearch to achieve a high-performance  interface service.
## Getting Started

Before running BeangoTown Server, you need to prepare the following infrastructure components, as they are essential for the project's operation:
* Elasticsearch
* Redis

The following command will clone BeangoTown Server into a folder. Please open a terminal and enter the following command:
```Bash
git clone https://github.com/Beangotown/beangotown-server
```

The next step is to build the project to ensure everything is working correctly. Once everything is built and configuration file is configured correctly, you can run as follows:

```Bash
# enter the beangotown-server folder
cd beangotown-server

# publish
 dotnet publish src/BeangoTownServer.HttpApi.Host/BeangoTownServer.HttpApi.Host.csproj -o beangotown/HttpApi

# enter beangotown folder
cd beangotown
# ensure that the configuration file is configured correctly

# run HttpApi service
 dotnet HttpApi/BeangoTownServer.HttpApi.Host.dll

```

After starting  the above service, BeangoTown Server is ready to provide external services.

## Modules

BeangoTown Server includes the following services:

- `BeangoTownServer.HttpApi.Host`: API interface service.

## Contributing

We welcome contributions to the BeangoTown Server project. If you would like to contribute, please fork the repository and submit a pull request with your changes. Before submitting a pull request, please ensure that your code is well-tested.


## License

BeangoTown Server is licensed under [MIT](https://github.com/Beangotown/beangotown-server/blob/master/LICENSE).
