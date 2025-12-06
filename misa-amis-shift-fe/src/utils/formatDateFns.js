import { format } from "date-fns";

/**
 * Định dạng ngày sang dạng dd/MM/yyyy.
 *
 * @author hiepnd
 * @param {string | Date} date - Ngày cần format.
 * @returns {string} Ngày đã được format hoặc chuỗi rỗng nếu dữ liệu không hợp lệ.
 */
export const formatDate = (date) => {
  if (!date) return "";
  return format(new Date(date), "dd/MM/yyyy");
};