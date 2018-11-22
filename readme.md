# tmpps.boardless

tmpps.boardless の CLI

## Required

- .Net core 2.1
- node.js

#### Options

- vscode
- Docker で Database を作成する場合
  - docker
  - docker-compose
  - linux

## Usage

#### 始めに

`npm install`

#### ビルド

`npm run build`

#### テスト

`npm run test`

#### Database を Docker で起動

`npm run db`

##### Docker 上の postgresql にログインする

`docker exec -ti database_boardless-postgres_1 psql -U postgres`

#### Debug

vscode でデバッグ実行可能

## リンク

- [ドキュメント](https://github.com/wakuwaku3/tmpps.boardless.docs)
