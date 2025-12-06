
/**
 * Lưu dữ liệu vào LocalStorage với key cho trước.
 *
 * Dữ liệu sẽ được tự động chuyển sang JSON string trước khi lưu.
 *
 * @author hiepnd
 * @param {string} key - Tên khóa dùng để lưu trữ.
 * @param {any} data - Dữ liệu cần lưu vào LocalStorage.
 * @returns {void}
 */
export const saveDataToLocalStorage = (key,data) =>{
    localStorage.setItem(key,JSON.stringify(data))
}

/**
 * Lấy dữ liệu từ LocalStorage theo key.
 *
 * Hàm sẽ parse JSON string thành object.
 *
 * Nếu key không tồn tại hoặc dữ liệu không hợp lệ,
 * hàm có thể trả về `null`.
 *
 * @author hiepnd
 * @param {string} key - Tên khóa cần lấy dữ liệu.
 * @returns {any|null} Dữ liệu đã parse hoặc null nếu không có.
 */
export const getDataFromLocalStorage = (key) => {
    return JSON.parse(localStorage.getItem(key));
}

