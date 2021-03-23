#!/usr/bin/env python
import time
from datetime import datetime
import argparse

import pika
from pymongo import MongoClient


def init_args():
    parser = argparse.ArgumentParser()
    parser.add_argument("--mongo_uri", type=str, help="MongoDB credentials")
    parser.add_argument("--mongo_db", type=str, default='logs', help="MongoDB db to use")
    parser.add_argument("--mongo_collection", type=str, default='raw', help="MongoDB collection to use")
    parser.add_argument("--buffer_size", type=int, default=3, help='how ofter to upload records into DB')
    parser.add_argument("--queue", type=str, default='logs', help="MongoDB collection to use")

    parser.add_argument("--host", type=str, default='54.90.58.148', help="RabbitMQ host to use")
    parser.add_argument("--rabbit_login", type=str, default='mylogin', help="RabbitMQ login")
    parser.add_argument("--rabbit_password", type=str, default='logspassword', help="RabbitMQ password")


    return parser.parse_args()


class Worker:
    def __init__(self, host, rabbit_login, rabbit_password, queue, buffer_size, mongo_uri, db, collection):
        self.host = host
        self.queue = queue

        self.rabbit_login = rabbit_login
        self.rabbit_password = rabbit_password

        self.buffer = []
        self.buffer_size = buffer_size


        client = MongoClient(mongo_uri)
        client.list_database_names()  # check DB connection
        self.collection = client[db][collection]


    def run(self):
        creds = pika.PlainCredentials(self.rabbit_login, self.rabbit_password)

        connection = pika.BlockingConnection(
        pika.ConnectionParameters(host=self.host, credentials=creds))
        channel = connection.channel()

        channel.queue_declare(queue=self.queue, durable=True)
        print(' [*] Waiting for messages. To exit press CTRL+C')


        # spread tasks over workers in case of many
        channel.basic_qos(prefetch_count=1)

        channel.basic_consume(queue=self.queue, on_message_callback=self.callback)
        channel.start_consuming()


    def process_message(self, message):
        self.buffer.append({
            'message': message,
            'time': datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
        })

        if len(self.buffer) % self.buffer_size == 0:
            print('uploading buffer')
            self.collection.insert_many(self.buffer)
            self.buffer = []


    def callback(self, channel, method, properties, body):
        # print(body, type(body.decode()))
        print(" [x] Received %r" % body.decode())
        self.process_message(body.decode())
        channel.basic_ack(delivery_tag=method.delivery_tag)



if __name__ == '__main__':
    args = init_args()
    w = Worker(args.host, args.rabbit_login, args.rabbit_password, args.queue, args.buffer_size, args.mongo_uri, args.mongo_db, args.mongo_collection)
    w.run()