const $ = (selector: string): HTMLElement | null =>
    document.querySelector(selector);

const $$ = (selector: string): NodeList<HTMLElement> =>
    document.querySelectorAll(selector);

const breadcrumb: HTMLElement = $('.breadcrumb');
const breadcrumbSteps: NodeList<HTMLElement> = $$('.breadcrumb__step');

[].forEach.call(
    breadcrumbSteps,
    (item: HTMLElement, index: number, array: HTMLElement[]): void => {
        item.onclick = (): void => {
            for (let i = 0, l = array.length; i < l; i++) {
                if (index >= i) {
                    array[i].classList.add('breadcrumb__step--active');
                } else {
                    array[i].classList.remove('breadcrumb__step--active');
                }
            }
        };
    },
);
