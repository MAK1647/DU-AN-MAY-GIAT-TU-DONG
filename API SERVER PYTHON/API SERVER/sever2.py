import time
#from http.server import BaseHTTPRequestHandler, HTTPServer#python 3
from BaseHTTPServer import BaseHTTPRequestHandler, HTTPServer #python 2
#from urllib.parse import urlparse, parse_qs#python 3
from  urlparse import urlparse, parse_qs#python 2
import serial
import sys
import mysql.connector
from mysql.connector import Error
from threading import Thread
import json
from io import BytesIO
from multiprocessing.pool import ThreadPool

HOST_NAME = '192.168.3.124'
PORT_NUMBER = 9000
mydb = mysql.connector.connect(
    host = "localhost",
    user = "root",
    passwd = "01696357178",  
    database = "mydb2"
	)
my_cursor = mydb.cursor()
ser = serial.Serial(
                #port = '/dev/ttyAMA0',
                port = 'COM9',
                baudrate = 9600,
                bytesize = serial.SEVENBITS,
                parity = serial.PARITY_EVEN,
                stopbits = serial.STOPBITS_ONE,
                timeout = 1
        )
ser.flushInput()
ser.flushOutput()
try:

    
    if mydb.is_connected():
        db_Info = mydb.get_server_info()
        print("Connected to MySQL Server version ", db_Info)
        cursor = mydb.cursor()
        cursor.execute("select database();")
        record = cursor.fetchone()
        print("Your connected to database: ", record)
except Error as e:
    print("Error while connecting to MySQL", e)


class MyHandler(BaseHTTPRequestHandler):
    ####
    def do_GET(self):
        ####
        path1 = urlparse(self.path).path # lay path
        print(path1)
        pool = ThreadPool(processes=10)
        response = BytesIO()
        
       
        if(path1 == '/read'):
            ###
            try:
                query2_components = parse_qs(urlparse(self.path).query) #lay query
                print(query2_components)
                    
                if(query2_components == {}):
                    
                        ###
                    #threadREAD1 = myThread2("readall",  7)
                    strread = 'ALL'
                    #threadREAD1.start()
                    twrv = ThreadWithReturnValue(target=self.read, args=(7,))
                    twrv.start()
                    #response.write(twrv.join())
                    print(twrv.join())
                    response.write(twrv.join())
                    #self.wfile.write(response.getvalue())
                else:
                    machine = query2_components['machine']
                    strread = ''.join(machine)
                    intpin = int(strread)
                    #self.readpin(int1)
                    #threadREAD2 = myThread2("readpin", intpin)
                    
                    #threadREAD2.start()
                    twrv = ThreadWithReturnValue(target=self.read, args=(intpin,))
                    twrv.start()
                    #response.write(twrv.join())
                    print(twrv.join())
                    response.write(twrv.join())
                    #self.wfile.write(response.getvalue())
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                
                #self.wfile.write(bytes(content3,'UTF-8'))
                self.wfile.write(response.getvalue())
                    
           
                
            except Exception as e:
                
                 
                print(e)
                ##
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                err_code=111
                error={"status": "FAIL", "error_code": 111}
                error_json=json.dumps(error)
                self.wfile.write(error_json)
                
        else:
            self.send_response(200)
            #self.send_header('Content-type', 'text/html')
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            content3 =content = '''
            <html><head><title>Title goes here.</title><link rel="icon" 
            type="image/png" href="http://example.com/myicon.png"></head>
            KO LAY DC PATH
            </body></html>
            '''
            
            err_code=112
            error={"status": "FAIL", "error_code": 112}
            error_json=json.dumps(error)
            self.wfile.write(error_json)
            #self.wfile.write(bytes(content3,'UTF-8'))
            #self.wfile.write(content3.encode('utf-8'))
            
        
    def printcmd(self,int1,int2,int3):
            b1 = int1
            c = int2
            d = int3
            
            if(b1 == 5 and c == 1):#chan Y4 may 5
                        
                b=":01050806FF00ED\r\n"
                thread1 = myThread("machine 5",b,  d)
                thread1.start()
                #print(b1+ " "+c)
           # elif(b1 == 5 and c == 0):
           #     b=":010500040000F6\r\n"
           #         #print(b1+ " "+c)
                
           # elif(b1 == 6 and c == 1):#chan Y5 may 6
           #     b=":01050005FF00F6\r\n"
                #print(b1+ " "+c)
           # elif(b1 == 6 and  c == 0):
            #    b=":010500050000F5\r\n"
                #print(b1+ " "+c)
                
            elif(b1 == 1 and c == 1):#chan Y0
                
                b=":01050801FF00F2\r\n"
                thread2 = myThread("machine 1",b,  d)
                thread2.start()
            #elif(b1 == 1 and c == 0):
                
             #   b=":010500000000FA\r\n"
                
            elif(b1==2 and c==1):#chan Y1
                
                b=":01050802FF00F1\r\n"
                thread3 = myThread("machine 2",b,  d)
                thread3.start()
            #elif(b1 == 2 and c==0):
             #   b=":010500010000F9\r\n"
                
                
            #elif(b1==3 and c==1):#chan Y2
             #   b=":01050002FF00F9\r\n"
           # elif(b1==3 and c==0):
            #    b=":010500020000F8\r\n"
                
            elif(b1==4 and c==1):#chan Y3
                b=":01050804FF00EF\r\n"
                thread4 = myThread("machine 4",b,  d)
                thread4.start()
           # elif(b1==4 and c==0):
            #    b=":010500030000F7\r\n"
                
            #elif(b1 == 6 and c==6):
             #   b=0
            else:
                b=1
                print("thong so nhap vao ko hop le3")
                
           # self.hello(b)# truyen b la chuoi va b1 la pin va d la so lan kich
    
    def  setupPLC(self,int1,int2):
        a=int1#machine int trong satabase
        b=int2#status int trong database
        print(a)
        print(b)
        if((a<7 and a>0) and (b>=0 and b<3)):
            try:
                my_sql = """UPDATE tiemmay SET loaimay = %s WHERE machine = %s"""
                my_cursor.execute(my_sql,(b,a,))#status,machine
                mydb.commit()
            except Error as e:
                print("Error while connecting to MySQL", e)
        else:
            print('thong so khong hop le2')
                        
    def writetime(self,a,b):
        lan2 = a
        command = b
        for n in range(lan2):
            ser.write(command.encode())
            time.sleep(0.4)
        ser.flushInput()
        ser.flushOutput()
    def read(self,a):
        pin=a
        ser.flushInput()
        ser.flushOutput()
        command2 = ":010100000008F6\r\n"
        ser.write(command2.encode())
        time.sleep(0.5)
        s3 = ser.read(12)
        s4=s3[7:-3]
        string2 = bin(int('1'+s4, 16))[3:]
        print("string la")
        print(string2)
        if(pin == 1):
            
            
            listpin0 = {"pin1" :  pin,"status1" : string2[7]}
            mylist = json.dumps(listpin0)
            print(mylist)
        elif(pin == 2): 
            
            listpin1 = {"pin2" :  pin,"status2" : string2[6]}
            mylist = json.dumps(listpin1)
            print(mylist)

        elif(pin ==3):
            listpin2 = {"pin3" :  pin,"status3" : string2[5]}
            mylist = json.dumps(listpin2)
            print(mylist)
            
        elif(pin ==4):
            listpin3 = {"pin4" :  pin,"status4" : string2[4]}
            mylist = json.dumps(listpin3)
            print(mylist)
            
        elif(pin ==5):
            listpin4 = {"pin5" :  pin,"status5" : string2[3]}
            mylist = json.dumps(listpin4)
            print(mylist)
            
        elif(pin ==6):
            listpin5 = {"pin" :  pin,"status" : string2[2]}
            mylist = json.dumps(listpin5)
            print(mylist)

        elif(pin == 7):
            listpin0 = {"pin1" :  1,"status1" : string2[7]}
            mylist0 = json.dumps(listpin0)
            print(mylist0)
            listpin1 = {"pin2" :  2,"status2" : string2[6]}
            mylist1 = json.dumps(listpin1)
            print(mylist1)
            listpin2 = {"pin3" :  3,"status3" : string2[5]}
            mylist2 = json.dumps(listpin2)
            print(mylist2)
            listpin3 = {"pin4" :  4,"status4" : string2[4]}
            mylist3 = json.dumps(listpin3)
            print(mylist3)
            listpin4 = {"pin5" :  5,"status5" : string2[3]}
            mylist4 = json.dumps(listpin4)
            print(mylist4)
            listpin5 = {"pin6" :  6,"status6" : string2[2]}
            mylist5 = json.dumps(listpin5)
            dict0 = json.loads(mylist0)
            dict1 = json.loads(mylist1)
            dict2 = json.loads(mylist2)
            dict3 = json.loads(mylist3)
            dict4 = json.loads(mylist4)
            dict5 = json.loads(mylist5)
            merged_dict = {key: value for (key, value) in (dict0.items() + dict1.items()+dict2.items()+dict3.items()+dict4.items()+dict5.items())}
            mylist = json.dumps(merged_dict)
            print(mylist)
        else:
            print("ko co ket qua pin")
            mylist="long"
        return mylist

    def do_PATCH(self):
        #path1 = urlparse(self.path).path
        response = BytesIO()
        try:
            ##
              
             
                #
                #query1_components = parse_qs(urlparse(self.path).query)
                #machine1 = query1_components['machine']
                #status1 = query1_components['status']
                #str1 = ''.join(machine1)#list thanh string
                #str2 = ''.join(status1)
                #int1 = int(str1)
                #int2 = int(str2)
            content_length = int(self.headers['Content-Length'])
            body = self.rfile.read(content_length)
            barcodeData_string= json.loads(body)
            print(json.dumps(barcodeData_string))
            int1 = int(barcodeData_string["machine"])
            int2 = int(barcodeData_string["status"])
            self.setupPLC(int1,int2)
            self.send_response(200)
            #self.send_header('Content-type', 'text/html')
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            content3 =content = '''
            <html><head><title>Title goes here.</title><link rel="icon" 
            type="image/png" href="http://example.com/myicon.png"></head>
                    
            </body></html>
            '''+"SETUP"+" "+barcodeData_string["machine"]+" "+barcodeData_string["status"]
            #self.wfile.write(bytes(content3,'UTF-8'))
            #self.wfile.write(content3.encode('utf-8'))
            print(json.dumps(barcodeData_string))
            self.wfile.write(json.dumps(barcodeData_string))
        except Exception as e :
            print(e)
                ##
            self.send_response(200)
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            content3 =content = '''
            <html><head><title>Title goes here.</title><link rel="icon" 
            type="image/png" href="http://example.com/myicon.png"></head>
            KO LAY DC QUERY
            </body></html>
            '''
            #self.wfile.write(bytes(content3,'UTF-8'))
            #self.wfile.write(content3.encode('utf-8'))
            #self.send_response(200)
            #self.end_headers()
            err_code=113
            error={"status": "FAIL", "error_code": 113}
            error_json=json.dumps(error)
            self.wfile.write(error_json)
        
        
    def do_POST(self):
        path1 = urlparse(self.path).path
        if(path1 == {}):
            print("ko co path")
        elif(path1 == '/setup' ):
             try:
                #
                #query1_components = parse_qs(urlparse(self.path).query)
                #machine1 = query1_components['machine']
                #status1 = query1_components['status']
                #str1 = ''.join(machine1)#list thanh string
                #str2 = ''.join(status1)
                #int1 = int(str1)
                #int2 = int(str2)
                content_length = int(self.headers['Content-Length'])
                body = self.rfile.read(content_length)
                barcodeData_string= json.loads(body)
                int1 = int(barcodeData_string["machine"])
                int2 = int(barcodeData_string["status"])
                self.setupPLC(int1,int2)
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                content3 =content = '''
                <html><head><title>Title goes here.</title><link rel="icon" 
                type="image/png" href="http://example.com/myicon.png"></head>
                    
                </body></html>
                '''+"SETUP"+" "+barcodeData_string["machine"]+" "+barcodeData_string["status"]
                #self.wfile.write(bytes(content3,'UTF-8'))
                #self.wfile.write(content3.encode('utf-8'))
                self.wfile.write(json.dumps(barcodeData_string))
             except Exception as e :
                print(e)
                ##
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                content3 =content = '''
                <html><head><title>Title goes here.</title><link rel="icon" 
                type="image/png" href="http://example.com/myicon.png"></head>
                KO LAY DC QUERY
                </body></html>
                '''
                #self.wfile.write(bytes(content3,'UTF-8'))
                #self.wfile.write(content3.encode('utf-8'))
                
                err_code=111
                error={"status": "FAIL", "error_code": 111}
                error_json=json.dumps(error)
                self.wfile.write(error_json)
        elif(path1 == '/write'):
            try:
                
            ####
                #query_components = parse_qs(urlparse(self.path).query) #lay query
                #print(query_components)
                try:
                    
                    #machine = query_components['machine']
                    #str1 = ''.join(machine)
                    content_length = int(self.headers['Content-Length'])
                    body = self.rfile.read(content_length)
                    barcodeData_string= json.loads(body)
                    #int1 = int(barcodeData_string["machine"])
                    #int2 = int(barcodeData_string["status"])
                    #int3 = int(barcodeData_string["time"])
                    machine1 = int(barcodeData_string["machine"])
                    print(type(machine1))
                    try:
                        
                        sql_select_query="""select * FROM tiemmay WHERE machine = %s"""
                        my_cursor.execute(sql_select_query,(machine1,))
                        result = my_cursor.fetchall()
                    except Error as e:
                        #
                        print("Error while connecting to MySQL", e)
                    for row in result:
                        break
                    typemachine=row[1] # 0 may giat , 1 may say ,2 chua setup
                    print(typemachine)
                except:
                    print("thong so nhap vao ko hop le2")
                if(typemachine == 1):#maysay
                    #
                    #status = query_components['status']
                    #lan = query_components['time']
                    str2 = "1"
                    str3 = barcodeData_string["time"]
                elif(typemachine == 0):#may giat
                    #status = query_components['status']
                    str2 = "1"
                    str3 = "1"
                    
                else:
                    print("ko to typemachine")
                
                int1 = machine1
                strmachine = str(int1)
                int2 = int(str2)
                int3 = int(str3)
                print(int1)
                print(int2)
                print(int3)
                
                #status_code=200
                self.printcmd(int1,int2,int3)
                #####
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                content3 =content = '''
                <html><head><title>Title goes here.</title><link rel="icon" 
                type="image/png" href="http://example.com/myicon.png"></head>
                
                </body></html>
                '''+"WRITE"+" "+strmachine+" "+str3
                #self.wfile.write(bytes(content3,'UTF-8'))
                #self.wfile.write(content3.encode('utf-8'))
                self.wfile.write(json.dumps(barcodeData_string))
                #print(json.dumps(barcodeData_string))
            except Exception as e :
                print(e)
                ##
                self.send_response(200)
                self.send_header('Content-type', 'application/json')
                self.end_headers()
                content3 =content = '''
                <html><head><title>Title goes here.</title><link rel="icon" 
                type="image/png" href="http://example.com/myicon.png"></head>
                ko lay dc query hoac may chua setup
                </body></html>
                '''
                #self.wfile.write(bytes(content3,'UTF-8'))
                #self.wfile.write(content3.encode('utf-8'))
                
                err_code=114
                error={"status": "FAIL", "error_code": 114}
                error_json=json.dumps(error)
                self.wfile.write(error_json)
    
            

   

class myThread(Thread):
    def __init__(self, name, lenh, lan):
        ####
            super(myThread, self).__init__()
            self.name= name
            self.lenh=lenh
            self.lan=lan
    def run(self):
        ####
            print(self.name)
            try:
                
                for n in range(self.lan):
                ####
                    ser.write(self.lenh.encode())
                    time.sleep(0.4)
            except:
                print("erroruart")
            #ser.flushInput()
            #ser.flushOutput()
 	    #print "%s: %s" % (self.name, time.ctime(time.time()))
class myThread2(Thread):
    def __init__(self, name, pin):
        super(myThread2, self).__init__()
        self.name= name
        #self.machine=machine
        self.pin=pin
    def run(self):
        print(self.name)
        print(self.pin)
        ser.flushInput()
        ser.flushOutput()
        command2 = ":010100000008F6\r\n"
        ser.write(command2.encode())
        time.sleep(0.5)
        s3 = ser.read(12)
        s4=s3[7:-3]
        string2 = bin(int('1'+s4, 16))[3:]
        print("string la")
        print(string2)
        if(self.pin == 1):
            
            
            listpin0 = {"pin" :  pin,"status" : string2[7]}
            mylist0 = json.dumps(listpin0)
            print(mylist0)
        elif(self.pin == 2):
            
            listpin1 = {"pin" :  pin,"status" : string2[6]}
            mylist1 = json.dumps(listpin1)
            print(mylist1)

        elif(self.pin ==3):
            listpin2 = {"pin" :  pin,"status" : string2[5]}
            mylist2 = json.dumps(listpin2)
            print(mylist2)
            
        elif(self.pin ==4):
            listpin3 = {"pin" :  pin,"status" : string2[4]}
            mylist3 = json.dumps(listpin3)
            print(mylist3)
            
        elif(self.pin ==5):
            listpin4 = {"pin" :  pin,"status" : string2[3]}
            mylist4 = json.dumps(listpin4)
            print(mylist4)
            
        elif(self.pin ==6):
            listpin5 = {"pin" :  pin,"status" : string2[2]}
            mylist5 = json.dumps(listpin5)
            print(mylist5)

        elif(self.pin == 7):
            listpin0 = {"pin" :  1,"status" : string2[7]}
            mylist0 = json.dumps(listpin0)
            print(mylist0)
            listpin1 = {"pin" :  2,"status" : string2[6]}
            mylist1 = json.dumps(listpin1)
            print(mylist1)
            listpin2 = {"pin" :  3,"status" : string2[5]}
            mylist2 = json.dumps(listpin2)
            print(mylist2)
            listpin3 = {"pin" :  4,"status" : string2[4]}
            mylist3 = json.dumps(listpin3)
            print(mylist3)
            listpin4 = {"pin" :  5,"status" : string2[3]}
            mylist4 = json.dumps(listpin4)
            print(mylist4)
            listpin5 = {"pin" :  6,"status" : string2[2]}
            mylist5 = json.dumps(listpin5)
            print(mylist5)
        else:
            print("ko co ket qua pin")
        
    
class ThreadWithReturnValue(Thread):
    
    def __init__(self, group=None, target=None, name=None,
                 args=(), kwargs={}, Verbose=None):
        #super(ThreadWithReturnValue, self).__init__()
        Thread.__init__(self, group, target, name, args, kwargs, Verbose)
        self._return = None
    def run(self):
        if self._Thread__target is not None:
            self._return = self._Thread__target(*self._Thread__args,
                                                **self._Thread__kwargs)
    def join(self):
        Thread.join(self)
        return self._return	    
 	#print "ket thuc vong lap", self.name


if __name__ == '__main__':
    #server_class = HTTPServer
    #httpd = server_class((HOST_NAME, PORT_NUMBER), MyHandler)
    #print(time.asctime(), 'Server Starts - %s:%s' % (HOST_NAME, PORT_NUMBER))
    #httpd.serve_forever()
    #server.socket.close()
    try:
        
        server_class = HTTPServer
        httpd = server_class((HOST_NAME, PORT_NUMBER), MyHandler)
        print(time.asctime(), 'Server Starts - %s:%s' % (HOST_NAME, PORT_NUMBER))
        httpd.serve_forever()
    except KeyboardInterrupt:
        pass
    httpd.server_close()
    print(time.asctime(), 'Server Stops - %s:%s' % (HOST_NAME, PORT_NUMBER))
        

  




            
