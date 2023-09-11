# buffer-message
buffer-message; message-pack

## 设计

为了方便弱类型语言对接，采用类型+(长度)+数据的形式

基础数据类型: 1 ByteType + Data
其他数据类型: 1 ByteType + 1 Byte Length + Data