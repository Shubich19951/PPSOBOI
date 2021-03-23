#!/usr/bin/env python
import sys
import argparse

import pika


def init_args():
    parser = argparse.ArgumentParser()
    parser.add_argument("--message", type=str, help="text message to store into DB")
    parser.add_argument("--host", type=str, default='54.90.58.148', help="RabbitMQ host to use")
    parser.add_argument("--queue", type=str, default='logs', help="MongoDB collection to use")
    parser.add_argument("--rabbit_login", type=str, default='mylogin', help="RabbitMQ login")
    parser.add_argument("--rabbit_password", type=str, default='logspassword', help="RabbitMQ password")

    return parser.parse_args()


class Sender:
    def __init__(self, host, rabbit_login, rabbit_password, queue):
        self.host = host
        self.queue = queue

        self.rabbit_login = rabbit_login
        self.rabbit_password = rabbit_password


    def send_message(self, message):

        creds = pika.PlainCredentials(self.rabbit_login, self.rabbit_password)

        connection = pika.BlockingConnection(
            pika.ConnectionParameters(host=self.host, credentials=creds))
        channel = connection.channel()

        channel.queue_declare(queue='task_queue', durable=True)

        channel.basic_publish(
            exchange='',
            routing_key=self.queue,
            body=message,
            properties=pika.BasicProperties(
                delivery_mode=2,  # make message persistent
            ))

        print(" [x] Sent message %r" % message)
        connection.close()


if __name__ == '__main__':
    args = init_args()

    s = Sender(args.host, args.rabbit_login, args.rabbit_password, args.queue)
    s.send_message(args.message)