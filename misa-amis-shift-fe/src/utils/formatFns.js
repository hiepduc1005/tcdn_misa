import { unref } from "vue"

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