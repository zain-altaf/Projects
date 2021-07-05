const request = require("request");
const express = require("express");
const bodyParser = require("body-parser");
const https = require("https");

const app = express();
app.use(express.static("public")); // this allows you to use static files by placing them inside a public folder
app.use(bodyParser.urlencoded({extended: true}));

app.get("/", function(req, res){
    res.sendFile(__dirname + "/signup.html");
});

app.post("/", function(req, res){
    const firstName = req.body.fName;
    const lastName = req.body.lName;
    const email = req.body.email;

    const data = {
        members: [
            {
                email_address: email,
                status: "subscribed",
                merge_fields: {
                    FNAME: firstName,
                    LNAME: lastName
                }
            }
        ]
    };

    const jsonData = JSON.stringify(data); // this will allow us to flat pack our JSON data object

    const url = "https://us6.api.mailchimp.com/3.0/lists/b5b1a6a0ab";
    const options = {
        method: "POST",
        auth: "zain1:f53d594f71a2db1f4f645e0a184423a3-us6"
    };

    const request = https.request(url, options, function(response){ // in order for us to send the jsonData flat pack to mailchimp we need to store the https in a constant and then we can send things over to the mailchimp server

        if(response.statusCode === 200){
            res.sendFile(__dirname + "/success.html");
        }
        else{
            res.sendFile(__dirname + "/failure.html");
        }

        response.on("data", function(data){
            console.log(JSON.parse(data)); // this will allow us to parse the response from mailchimp from flat packed to JSON object
        });
    }); // this will take a URL, options ie over here we want to use the POST method and then it will have a callback that will recieve a response from the mailchimp server

    request.write(jsonData); // passing the json data to the mailchimp server
    request.end(); // end the request

});

app.listen(3000, function(){
    console.log("The website is running on port 3000");
});

//API key
//f53d594f71a2db1f4f645e0a184423a3-us6

//unique id for audiences
//b5b1a6a0ab