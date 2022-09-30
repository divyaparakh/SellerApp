# E-Auction Application
    1.Architecture
    2.Seller
    3.Buyer
    4.Auth
    5.Ocelot ApiGateWay
    6.Angular Portal


# 1.Architecture

![Architecure](docs/Arch-daigram.png)


# 2.Seller Api

APIEndPoint : **https://localhost:5002**   **https://eauctionapplicationseller.azurewebsites.net**

GateWayEndPoint : **https://localhost:5000/e-auction**  **https://eauctionapplicationoceletapigateway.azurewebsites.net/e-auction**

Here we are used **Products** container in cosmos db with partition key was  **./Category** ,

here the sample cosmos document

    {
        "ProductName": "MRF Bat",
        "ShortDescription": "MRF Bat",
        "DetailedDescription": "Strong Bat",
        "Category": "Painting",
        "StartingPrice": 100,
        "BidEndDate": "2022-10-09T14:14:45.882Z",
        "Seller": {
            "FirstName": "Trimurthulu",
            "LastName": "Ravi",
            "Address": "Vizag",
            "City": "Vizag",
            "State": "AP",
            "Pin": "531036",
            "Phone": "7306618683",
            "Email": "09trimurthulu81@gmail.com"
        },
        "id": "Painting:774aea12-da0d-4afd-8bb8-e844ce6b4c53",
        "_rid": "SiMxAOJ-MB8BAAAAAAAAAA==",
        "_self": "dbs/SiMxAA==/colls/SiMxAOJ-MB8=/docs/SiMxAOJ-MB8BAAAAAAAAAA==/",
        "_etag": "\"00000000-0000-0000-c5e1-561c470d01d8\"",
        "_attachments": "attachments/",
        "_ts": 1662902461
    }

And we did one internal http call for buyer api to get the bids information

# 3.Buyer Api

APIEndPoint : **https://localhost:5004**  **https://eauctionapplicationbuyer.azurewebsites.net**

GateWayEndPoint : **https://localhost:5000/e-auction**  **https://eauctionapplicationoceletapigateway.azurewebsites.net/e-auction**

Here we are used **ProductBids** container in cosmos db with partition key was  **./ProductId** ,

here the sample cosmos document

    {
        "FirstName": "Trimurthulu",
        "LastName": "Ravi",
        "Address": "Vizag",
        "City": "Vizag",
        "State": "AP",
        "Pin": "531036",
        "Phone": "7306618683",
        "Email": "09trimurthulu81@gmail.com",
        "BidAmount": 200,
        "ProductId": "Painting:774aea12-da0d-4afd-8bb8-e844ce6b4c53",
        "id": "Painting:774aea12-da0d-4afd-8bb8-e844ce6b4c53/5d3f8901-fc7b-4dd2-a02c-93729de1280f",
        "_rid": "SiMxAIMWDwwBAAAAAAAAAA==",
        "_self": "dbs/SiMxAA==/colls/SiMxAIMWDww=/docs/SiMxAIMWDwwBAAAAAAAAAA==/",
        "_etag": "\"00000000-0000-0000-c794-0afd38c801d8\"",
        "_attachments": "attachments/",
        "_ts": 1663089166
    }

And we did one internal http call for seller api to get the product information for validation usecase.

# 4.Auth Api

APIEndPoint : **https://localhost:5006**  **https://eauctionapplicationauth.azurewebsites.net**

GateWayEndPoint : **https://localhost:5000/e-auction**  **https://eauctionapplicationoceletapigateway.azurewebsites.net/e-auction**

here we are simple login to genrate jwt used, this is payload

Request

    {
    "username":"rtrimurthulu",
    "password":"rtrimurthulu"
    }

Response

    {
        "message": "Record successfully created.",
        "isError": false,
        "result": {
            "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJ0cmltdXJ0aHVsdSIsImF1ZCI6WyJhcGkuc2VsbGVyLmNvbSIsImFwaS5idXllci5jb20iLCJhcGkuYXV0aC5jb20iXSwiZXhwIjoxNjYzMTYxMDU4LCJpc3MiOiJhcGkuYXV0aC5jb20ifQ.ETc2fn-IR8s1ntN0FWJMbn2ViEVglB2IqtTOcQNe8iQ",
            "isSuccess": true
            }
        }

# 5. Ocelot APIGateway

we did ocelot configuration json for all exposed endpoints with downstramendpoints are buyer and seller and auth

GateWayEndPoint : **https://localhost:5000**  **https://eauctionapplicationoceletapigateway.azurewebsites.net/**


we are taken talend api tester collection [see here e-auction-collection.json](docs/e-auction-collection.json) 
we are taken talend api tester collection after cloud deploy [see here e-auction-collection.json](docs/cloud-eauction-all-collections) 



# 6. Angular Portal

we are deployed angular application as static webapp in azure.

Angular Portal urls : **https://localhost:4200**  **https://white-island-03fe90310.1.azurestaticapps.net/**