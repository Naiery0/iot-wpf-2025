## 파이썬 MQTT Publish 
# paho-mqtt 라이브러리 설치
# pip install paho-mqtt
import paho.mqtt.client as mqtt
import json
import datetime as dt
import time
import uuid
from collections import OrderedDict
import random

PUB_ID = 'IOT64' # 본인 아이피 마지막주소
BROKER = '210.119.12.64' # 본인 아이피
PORT = 1883
TOPIC = 'smarthome/64/topic'  # publish/subscribe에서 사용할 토픽
COLORS = ['RED', 'ORANGE', 'YELLOW', 'GREEN', 'BLUE', 'NAVY', 'PURPLE']
COUNT = 0

# [Fake] 센서 설정
SENSOR1 = "온습도센서세팅"; PIN1 = 5
SENSOR2 = "포토센서세팅"; PIN2 = 7
SENSOR3 = "워터드롭센서세팅"; PIN3 = 9
SENSOR4 = "인체감지센서세팅"; PIN4 = 11


# 연결 콜백
def on_connect(client, userdata, flags, reason_code, properties=None):
    print(f'Connected with reason code : {reason_code}')    

# 퍼블리시 완료후 발생 콜백
def on_publish(client, userdata, mid):
    print(f'Message published mid : {mid}')

try:
    client= mqtt.Client(client_id=PUB_ID, protocol=mqtt.MQTTv5)
    client.on_connect = on_connect
    client.on_publish = on_publish

    client.connect(BROKER, PORT)
    client.loop_start() 

    while True:
        # 퍼블리시 
        currtime = dt.datetime.now()
        selected = random.choice(COLORS)
        temp =  random.uniform(20.0, 29.9)
        humid =  random.uniform(20.0, 29.9)
        rain = random.randint(0,1)
        detect = random.randint(0,1)
        photo = random.randint(50,255)


                ## 센싱데이터를 json형태로 변경
        ## OrderedDict로 먼저 구성. 순서가 있는 딕셔너리타입 객체
        raw_data = OrderedDict()
        raw_data['PUB_ID'] = PUB_ID
        raw_data['COUNT'] = COUNT
        raw_data['SENSING_DT'] = currtime.strftime(f'%Y-%m-%d %H:%M:%S')  # C# 'yyyy-MM-dd HH:mm:ss
        raw_data['TEMP'] = f'{temp:.1f}'
        raw_data['HUMID'] = f'{humid}'
        raw_data['LIGHT'] = 1 if photo >= 200 else 0
        raw_data['HUMAN'] = detect
        # Python 딕셔너리 형태로 저장되어 있음. json이랑 거의 똑같음

        ## OrderedDict -> json 타입으로 변경
        pub_data = json.dumps(raw_data, ensure_ascii=False, indent='\t')

        ## payload에 json 데이터를 첨부
        client.publish(TOPIC, payload=pub_data, qos=1)
        time.sleep(1)


except Exception as ex:
    print(f'Error raised : {ex}')
except KeyboardInterrupt:
    print('MQTT 전송중단')
    client.loop_stop()
    client.disconnect()