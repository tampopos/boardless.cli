# boardless.deploys.db

db の展開

## Required

- .Net core 2.2
- node.js

#### Options

- vscode
- Docker で Database を作成する場合
  - docker
  - docker-compose
  - linux

## Usage

#### restore

`dotnet restore Deploys.Db/`

#### build

`dotnet build Deploys.Db/`

#### run

`dotnet run -p Deploys.Db/`

#### test

`sudo docker-compose up --build`

#### debug

vscode でデバッグ実行可能

#### validate ci config

`circleci config validate`

#### release

```bash
git tag X.Y.Z
git push origin --tags
```

## リンク

- [ドキュメント](https://github.com/wakuwaku3/tmpps.boardless.docs)
