# E-Auction Application
    1.Architecture
    2.SellerApp
    3.BuyerApp
    4.Auth
    5.ApiGateWay
    6.Web Portal (React)

# 1.Architecture
![image](https://user-images.githubusercontent.com/31089416/194471303-fd5f7639-3a0f-48cd-b0ec-ae22e31568bc.png)

# 2.SellerApp 

APIEndPoint : **https://sellerapp20221003120442.azurewebsites.net**

GateWayEndPoint : **https://apigateway20221003120814.azurewebsites.net/seller/show-bids**

Here we are used **Seller** and **Products** container in mongo db with partition key was  **SellerId**

here the sample mongodb document
**Seller**
    {
  "_id": {
    "$oid": "6329a81eac54cb3fea187d3e"
  },
  "FirstName": "Divya",
  "LastName": "Parakh",
  "Address": "Chectak Apartment",
  "City": "Udaipur",
  "State": "Rajasthan",
  "Pin": "313001",
  "Phone": "8955458807",
  "Email": "parakhdivya0507@gmail.com"
}
**Product**
{
  "_id": {
    "$oid": "633682ce9d581a02d7586a3f"
  },
  "ProductName": "Car Painting",
  "ShortDescription": "cars",
  "DetailedDescription": "Car Painting",
  "Category": "Painting",
  "StartingPrice": "567",
  "BidEndDate": "30-Dec-2022",
  "SellerId": {
    "$oid": "6329a81eac54cb3fea187d3e"
  }
}

# 3.BuyerApp

APIEndPoint : **https://buyerapp20221003103132.azurewebsites.net**

GateWayEndPoint : **https://apigateway20221003120814.azurewebsites.net/buyer/place-bid**

Here we are used **ProductBid** container in mongo db with partition key was  **./ProductId** ,

here the sample mongo document

    {
  "_id": {
    "$oid": "633297a6170ada9c133e126e"
  },
  "FirstName": "divya",
  "LastName": "parakh",
  "Address": "Address",
  "City": "City",
  "State": "State",
  "Pin": "313001",
  "Phone": "8888888888",
  "Email": "p@dfdf.com",
  "ProductId": {
    "$oid": "632b045db10c25d497cc23a5"
  },
  "BidAmount": "2222"
}

# 4.Auth

APIEndPoint : **https://authservice20221003101403.azurewebsites.net**

GateWayEndPoint : **https://apigateway20221003120814.azurewebsites.net/auth/user**

here we have a post login call to genrate jwt token, this is payload

Request

    {
    "username":"username",
    "password":"password"
    }

Response

   {
    "code": 200,
    "message": "Success",
    "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsiYXBpLnNlbGxlci5jb20sYXBpLmJ1eWVyLmNvbSIsImFwaS5zZWxsZXIuY29tLGFwaS5idXllci5jb20iXSwiaXNzIjoiYXBpLmF1dGguY29tIiwidW5pcXVlX25hbWUiOiJ1c2VybmFtZSIsImV4cCI6MTY2NDUxNzM4MX0.2a-xxtUgk9v2SBiz3lOCovAip0D7or64mnLwSMORZkc"
}

# 5. APIGateway

we did ocelot configuration json for all exposed endpoints with downstramendpoints are buyer and seller and auth

GateWayEndPoint : **https://apigateway20221003120814.azurewebsites.net**



# 6. Web Portal(React)

we are deployed react application as static webapp in azure.

React Portal urls : **https://webportalreact.azurewebsites.net/**
