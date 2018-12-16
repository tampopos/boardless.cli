result=$(curl -X POST 127.0.0.1:80/migration -H "Content-Length:0")
echo $result
if [ $result -eq '0' ]; then
  echo '成功'
  exit 0
else
  echo '失敗'
  exit 1
fi