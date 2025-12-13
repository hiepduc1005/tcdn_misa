/**
 * Key dùng để lưu trạng thái thu gọn sidebar vào LocalStorage.
 *
 * @author hiepnd
 * @since 2025-10-05
 */
export const STORAGE_KEY = 'sidebar_collapsed_state';

/**
 * Danh sách các tùy chọn số lượng bản ghi / trang.
 *
 * Dùng cho bộ chọn page size trong bảng.
 *
 * @type {{label: string, value: number}[]}
 * @author hiepnd
 * @since 2025-10-05
 */
export const PAGE_SIZE_OPTIONS = [
    {
        label: '10',
        value: 10,
    },
    {
        label: '20',
        value: 20,
    },
    {
        label: '30',
        value: 30,
    },
    {
        label: '50',
        value: 50,
    },
    {
        label: '100',
        value: 100,
    }
]


/**
 * Các toán tử filter dành cho trường kiểu text/string.
 *
 * Áp dụng cho component lọc dữ liệu.
 *
 * @type {{value: string, label: string}[]}
 * @author hiepnd
 * @since 2025-10-05
 */
export const TEXT_FILTER_OPERATORS = [
    { value: 'empty', label: '(Trống)' },
    { value: 'not_empty', label: '(Không trống)' },
    { value: 'contains', label: 'Chứa' },
    { value: 'eq', label: 'Bằng' },
    { value: 'neq', label: 'Khác' },
    { value: 'not_contains', label: 'Không chứa' },
    { value: 'starts_with', label: 'Bắt đầu với' },
    { value: 'ends_with', label: 'Kết thúc với' }
];

export const COLUMN_TYPE = {
    TEXT: 'text',
    NUMBER: 'number',
    SELECT: 'select',
    DATE : 'date',
}

/**
 * Các toán tử filter dành cho number và date.
 *
 * @type {{value: string, label: string}[]}
 * @author hiepnd
 * @since 2025-11-05
 */
export const NUMBER_DATE_FILTER_OPERATORS = [
    { value: 'eq', label: 'Bằng' },
    { value: 'neq', label: 'Khác' },
    { value: 'lt', label: 'Nhỏ hơn' },
    { value: 'lte', label: 'Nhỏ hơn hoặc bằng' },
    { value: 'gt', label: 'Lớn hơn' },
    { value: 'gte', label: 'Lớn hơn hoặc bằng' },
    { value: 'empty', label: '(Trống)' },
    { value: 'not_empty', label: '(Không trống)' }
];

/**
 * Danh sách action cho bảng Shift.
 *
 * Dùng để hiển thị menu thao tác (duplicate, active, delete).
 *
 * @type {{key: string, label: string, iconClass: string}[]}
 * @author hiepnd
 * @since 2025-12-05
 */
export const SHIFT_TABLE_ACTION = [
    {
        key: "duplicate",
        label: "Nhân bản",
        iconClass: "duplicate-icon"
    },
    {
        key: "toggle_active",
        label: "Sử dụng", // hoặc Ngừng sử dụng tùy row
        iconClass: "active-icon"
    },
    {
        key: "delete",
        label: "Xóa",
        iconClass: "trash-red-icon",
    }
]


export const SHIFT_MODAL_TYPE = {
    CREATE: "create",
    UPDATE: "update"
}
