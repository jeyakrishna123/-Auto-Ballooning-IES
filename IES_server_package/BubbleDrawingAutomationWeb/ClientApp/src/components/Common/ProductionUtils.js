/**
 * Production Environment Utilities
 * Handles production-specific rendering issues and provides consistent re-render functions
 */

/**
 * Detects if the current environment is production
 * @returns {boolean} True if production environment
 */
export const isProduction = () => {
    return process.env.NODE_ENV === 'production';
};

/**
 * Forces a Konva stage to redraw with production environment handling
 * @param {Object} stage - Konva stage object
 * @param {number} delay - Delay before redraw (default: 10ms)
 */
export const forceStageRedraw = (stage, delay = 10) => {
    if (!stage) return;
    
    try {
        if (isProduction()) {
            // More aggressive redraw in production
            stage.batchDraw();
            setTimeout(() => {
                stage.batchDraw();
            }, delay);
        } else {
            stage.batchDraw();
        }
    } catch (error) {
        console.warn('Stage redraw failed:', error);
    }
};

/**
 * Forces a Konva layer to redraw with production environment handling
 * @param {Object} layer - Konva layer object
 * @param {number} delay - Delay before redraw (default: 10ms)
 */
export const forceLayerRedraw = (layer, delay = 10) => {
    if (!layer) return;
    
    try {
        if (isProduction()) {
            // More aggressive redraw in production
            layer.batchDraw();
            setTimeout(() => {
                layer.batchDraw();
            }, delay);
        } else {
            layer.batchDraw();
        }
    } catch (error) {
        console.warn('Layer redraw failed:', error);
    }
};

/**
 * Forces a DOM element with Konva stage to redraw
 * @param {string} selector - CSS selector for the stage element
 * @param {number} delay - Delay before redraw (default: 25ms)
 */
export const forceStageElementRedraw = (selector, delay = 25) => {
    try {
        const stageElement = document.querySelector(selector);
        if (stageElement && stageElement.__konva) {
            if (isProduction()) {
                // More aggressive redraw in production
                stageElement.__konva.batchDraw();
                setTimeout(() => {
                    stageElement.__konva.batchDraw();
                }, delay);
            } else {
                stageElement.__konva.batchDraw();
            }
        }
    } catch (error) {
        console.warn('Stage element redraw failed:', error);
    }
};

/**
 * Debounced redraw function for production environments
 * @param {Function} redrawFunction - Function to call for redraw
 * @param {number} delay - Delay in milliseconds (default: 100ms)
 * @returns {Function} Debounced function
 */
export const createDebouncedRedraw = (redrawFunction, delay = 100) => {
    let timeoutId;
    return (...args) => {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => {
            redrawFunction(...args);
        }, delay);
    };
};

/**
 * Production-safe setTimeout with error handling
 * @param {Function} callback - Function to execute
 * @param {number} delay - Delay in milliseconds
 * @returns {number} Timeout ID
 */
export const productionSafeTimeout = (callback, delay) => {
    try {
        return setTimeout(() => {
            try {
                callback();
            } catch (error) {
                console.warn('Production timeout callback failed:', error);
            }
        }, delay);
    } catch (error) {
        console.warn('Production timeout creation failed:', error);
        return null;
    }
};

/**
 * Forces multiple redraws for production environments
 * @param {Array} redrawFunctions - Array of redraw functions
 * @param {number} baseDelay - Base delay between redraws (default: 25ms)
 */
export const forceMultipleRedraws = (redrawFunctions, baseDelay = 25) => {
    if (!isProduction()) {
        // In development, just call once
        redrawFunctions.forEach(fn => {
            try {
                fn();
            } catch (error) {
                console.warn('Redraw function failed:', error);
            }
        });
        return;
    }
    
    // In production, call multiple times with delays
    redrawFunctions.forEach((fn, index) => {
        productionSafeTimeout(() => {
            try {
                fn();
            } catch (error) {
                console.warn('Production redraw function failed:', error);
            }
        }, baseDelay * (index + 1));
    });
};
