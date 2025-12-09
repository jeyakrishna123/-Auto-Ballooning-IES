import axios from "axios";

const config = {
    ENVIRONMENT: process.env.REACT_APP_ENV,
    BASE_URL: process.env.REACT_APP_SERVER,
    console: process.env.REACT_APP_CONSOLE === "true",
};

export const instance = axios.create({
    baseURL: config.BASE_URL,
    withCredentials:true,    //preflight cors error in deployemnt - add
    headers: {
      //  "Access-Control-Allow-Origin": "*",
      //  "Access-Control-Allow-Headers":"*", 
        // 'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',      //preflight cors error in deployemnt - cmd
        // "Access-Control-Allow-Credentials":true,                                    //preflight cors error in deployemnt - cmd
        'Accept': 'application/json',
        "Content-type": "application/json"
    }
});


