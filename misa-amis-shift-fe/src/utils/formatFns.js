import { unref } from "vue"
import { round } from "lodash"

/**
 * Chuyển một đối tượng Proxy (reactive/ref của Vue)
 * thành một plain object thông thường.
 *
 * Sử dụng `unref` để lấy giá trị gốc,
 * sau đó dùng JSON.parse(JSON.stringify(...))
 * để loại bỏ Proxy, reactive và mọi getter.
 *
 * @author hiepnd
 * @param {any} proxyObj - Đối tượng Proxy hoặc reactive/ref của Vue.
 * @returns {object} Plain object đã được chuyển đổi.
 */
export const convertToPlainObject = (proxyObj) => {
    return JSON.parse(JSON.stringify(unref(proxyObj)))
}

export function roundNumber(n) {
    return round(n, 1); // Làm tròn 1 chữ số thập phân
}

/**
 * Convert kiểu time  (hh:mm:ss) nhận được từ server thành hh:mm 
 * 
 *
 * @author hiepnd
 * @returns {object} Time đã được chuyển đổi.
 */
export const formatTimeToHHMM = (time) => {
    if (typeof time === "number") {
        return time;
    }
    
    // "05:30:00" -> "05:30"  [start,end)
    return time.substring(0, 5);
};

export const camelToPascalCase = (value) => {
    if (!value) return "";
    // Chữ cái đầu viết hoa + phần còn lại giữ nguyên
    return value.charAt(0).toUpperCase() + value.slice(1)
}