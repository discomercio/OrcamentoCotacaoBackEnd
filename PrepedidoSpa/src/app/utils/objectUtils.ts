export class ObjectUtils {
    public static isEmpty(obj: Object): boolean {
        return Object.entries(obj).length === 0;
    }
}

