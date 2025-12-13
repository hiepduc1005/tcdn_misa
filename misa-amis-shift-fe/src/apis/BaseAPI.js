import api from "./APIConfig";

export default class BaseAPI {
    constructor() {
        this.controler = null;
    }
    /**
     * Phương thức lấy tất cả dữ liệu
     */
    getAll() {
        return api.get(`${this.controler}`);
    }

    exportExcel(payload){
        return api.post(`${this.controler}/export-excel`, payload ,{
            responseType: 'blob'
        })
    }

     /**
     * Hàm lấy dữ liệu theo id
     * @param {*} id 
     */
    getById(id) {
        return api.get(`${this.controler}/${id}`);
    }

    /**
     * Hàm lấy dữ liệu phân trang
     * @param {*} payload 
     */
    paging(payload) {
        return api.post(`${this.controler}/datapaging`, payload);
    }

    /**
     * Hàm thêm mới dữ liệu
     * @param {*} body 
     */
    insert(body) {
        return api.post(`${this.controler}`, body);
    }

    /**
     * Hàm cập nhật dữ liệu
     * @param {*} id 
     * @param {*} body 
     */
    update(body) {
        return api.put(`${this.controler}`, body);
    }
    /**
     * Hàm xóa bản ghi
     * @param {*} id 
     */
    delete(id) {
        return api.delete(`${this.controler}/${id}`);
    }
}