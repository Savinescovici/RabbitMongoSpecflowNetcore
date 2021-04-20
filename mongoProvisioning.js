db = db.getSiblingDB('mydatabase')
db.createCollection("users")
db.createUser(
    {
        user: "mongodev",
        pwd: "mongodev",
        roles: [
            { role: "readWrite", db: "mydatabase" }
        ]
    }
);