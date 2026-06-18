using System;

namespace DeteksiBanjir.Services;

public class FuzzyService
{
    // Fuzzy Logic implementation from scratch
    // Input: WaterLevel (m), Rainfall (mm/hr), Discharge (m3/s)
    // Output: RiskScore (0-10) and Category (No/Yellow/Orange/Red Alert)

    public (double RiskScore, string Category) CalculateRisk(double waterLevel, double rainfall, double discharge)
    {
        // 1. Fuzzification
        var wlLow = FuzzifyLow(waterLevel, 2, 5);
        var wlMedium = FuzzifyMedium(waterLevel, 3, 6, 8);
        var wlHigh = FuzzifyHigh(waterLevel, 6, 9);

        var rLow = FuzzifyLow(rainfall, 10, 30);
        var rMedium = FuzzifyMedium(rainfall, 20, 50, 80);
        var rHigh = FuzzifyHigh(rainfall, 60, 100);

        var dLow = FuzzifyLow(discharge, 30, 60);
        var dMedium = FuzzifyMedium(discharge, 50, 80, 110);
        var dHigh = FuzzifyHigh(discharge, 90, 130);

        // 2. Rule Evaluation (simplified rules)
        // Rule 1: If WL is High and R is High and D is High then Risk is High
        var riskHigh = Math.Min(wlHigh, Math.Min(rHigh, dHigh));
        
        // Rule 2: If WL is Medium and R is Medium then Risk is Medium
        var riskMedium = Math.Min(wlMedium, rMedium);
        
        // Rule 3: If WL is Low and R is Low then Risk is Low
        var riskLow = Math.Min(wlLow, rLow);
        
        // We can add more rules, taking max for aggregation of same consequents
        // Here we just use these three for demonstration

        // 3. Defuzzification (Centroid method)
        // Center values for Risk: Low = 2, Medium = 5, High = 8
        double numerator = (riskLow * 2) + (riskMedium * 5) + (riskHigh * 8);
        double denominator = riskLow + riskMedium + riskHigh;

        double riskScore = denominator == 0 ? 0 : numerator / denominator;

        // 4. Categorization
        string category;
        if (riskScore < 3.0) category = "No Alert";
        else if (riskScore < 6.0) category = "Yellow Alert";
        else if (riskScore < 8.0) category = "Orange Alert";
        else category = "Red Alert";

        return (Math.Round(riskScore, 2), category);
    }

    private double FuzzifyLow(double x, double a, double b)
    {
        if (x <= a) return 1.0;
        if (x > a && x < b) return (b - x) / (b - a);
        return 0.0;
    }

    private double FuzzifyMedium(double x, double a, double b, double c)
    {
        if (x <= a || x >= c) return 0.0;
        if (x > a && x <= b) return (x - a) / (b - a);
        if (x > b && x < c) return (c - x) / (c - b);
        return 0.0;
    }

    private double FuzzifyHigh(double x, double a, double b)
    {
        if (x <= a) return 0.0;
        if (x > a && x < b) return (x - a) / (b - a);
        return 1.0;
    }
}
