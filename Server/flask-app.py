from flask import Flask, escape, request, render_template, url_for, jsonify
import json, random, string, hashlib, binascii
import mysql.connector as mariadb

# To check available hash type
# print(dir(hashlib))

app = Flask(__name__)
# Database name, database username, database password and database table
db = 'login';
dbun = 'login';
dbpw = '123456789';
dbTable =  'account';

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/register', methods=['POST'])
def register():
    data = request.get_json()
    if(len(data["id"]) < 5):
        print("User name has to be longer than 4.")
        return "User name has to be longer than 4."
    if(len(data["password"]) < 8):
        print("Password has to be longer than 7.")
        return "Password has to be longer than 7."

    mariadb_connection = mariadb.connect(host='localhost', port=3306, user=dbun, password=dbpw, database=db)
    cursor = mariadb_connection.cursor()
    cursor.execute("SELECT * FROM " + dbTable + " WHERE User='" + data["id"] + "'")
    records = cursor.fetchall()
    if not records:
        salt = randomString(128)
        # dk = hashlib.pbkdf2_hmac('sha512', data["password"].encode('utf-8'), salt.encode('utf-8'), 10000)
        # hash = binascii.hexlify(dk).decode("utf-8")
        hash = hashlib.sha3_512((data["password"] + salt).encode("utf-8")).hexdigest()
        # print("INSERT INTO " + dbTable + " (User, Salt, Hash) VALUES ('" + data["id"] + "', '" + salt + "', '" + hash + "');")
        cursor.execute("INSERT INTO " + dbTable + " (User, Salt, Hash) VALUES ('" + data["id"] + "', '" + salt + "', '" + hash + "');")
        mariadb_connection.commit()
        print("Registered")
        return "Registered"
    else:
        print("The username already exists.")
        return "The username already exists."

    """
    for row in records:
        print("User = ", row[0])
        print("Salt = ", row[1])
        print("Hash  = ", row[2])
    """
    # closing the cursor
    cursor.close()
    # closing database connection.
    if(mariadb_connection.is_connected()):
        mariadb_connection.close()
        print("MySQL connection is closed")

    # saving data
    # mariadb_connection.commit()
    # discard chages
    # mariadb_connection.rollback()
    print("Unexpected error. \n Please try again later.")
    return "Unexpected error. \n Please try again later."

def randomString(stringLength):
    lettersAndDigits = string.ascii_letters + string.digits
    # + string.punctuation
    return ''.join(random.choice(lettersAndDigits) for i in range(stringLength))

@app.route('/login', methods=['POST'])

@app.after_request
def add_header(response):
    """
    Add headers to both force latest IE rendering engine or Chrome Frame,
    and also to cache the rendered page for 10 minutes.
    """
    response.headers['X-UA-Compatible'] = 'IE=Edge,chrome=1'
    response.headers['Cache-Control'] = 'public, max-age=0'
    return response

if __name__ == "__main__":
    app.run(debug=True)
