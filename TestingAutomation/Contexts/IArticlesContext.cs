using System.Collections.Generic;

namespace TestFramework.Contexts
{
    public interface IArticlesContext
    {
        Dictionary<string, string> GetRegularItem();
        string GetRegularItemBarcode();
        string GetPromotionItemBarcode();
        string GetDiscountItemBarcode();
        string GetPriceVariableItemBarcode();
        string GetPriceVariableInBarcodeItemBarcode();
        string GetWeightedItemBarcode();
        string GetWeightedInBarcodeItemBarcode();
        string GetTraceabilityItemBarcode();
        string GetTraceabilityInBarcodeItemBarcode();
        string GetDepositItemBarcode();
        string GetHamperItemBarcode();
        string GetCannotBeSoldItemBarcode();
        string GetInvalidItemBarcode();
    }
}
