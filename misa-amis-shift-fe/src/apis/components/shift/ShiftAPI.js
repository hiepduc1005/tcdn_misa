import api from "../../APIConfig";
import BaseAPI from "../../BaseAPI";

class ShiftAPI extends BaseAPI{
    constructor(){
        super();
        this.controler = "api/shift"
    }

    activeAll(shiftIds) {
        return api.post(`${this.controler}/active`, shiftIds);
    }

    inactiveAll(shiftIds) {
        return api.post(`${this.controler}/inactive`, shiftIds);
    }

    deleteShifts(shiftIds) {
        return api.post(`${this.controler}/delete`, shiftIds );
    }

   

}

export default new ShiftAPI();