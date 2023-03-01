# !/usr/bin/env python
import socket,threading
Zero=False
def message_return(s):
  while True:
    global msg
    msg = s.recv(1024).decode()
    print('\nreturn %s'%msg)
    if msg == 'True':
      global Zero
      Zero = True

def main():
  # create threads

  s = socket.socket()
  hostname = '192.168.1.100'# Server IP/Hostname\n",
  port = 8000  # Server Port\n",
  s.connect((hostname, port))  # Connects to server\n",
  t = threading.Thread(target=message_return, args=(s,))
  t.start()

  while True:

    global Zero
    if Zero == True:
      x = "20,25,12"
      s.send(x.encode())
      break



if __name__ == '__main__':
  main()
