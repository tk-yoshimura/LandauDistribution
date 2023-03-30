# LandauDistribution

In probability theory, the Landau distribution is a probability distribution named after Lev Landau.  
Because of the distribution's "fat" tail, the moments of the distribution,  
like mean or variance, are undefined. The distribution is a particular case of stable distribution.

## Definition

The original Landau distribution defined by Landau can be evaluated on real numbers as follows:

![define origin](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_origin.svg)

The Landau distribution, generalized to a stable distribution by introducing position and scale parameters, is as follows:

![define stabledist](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_stabledist_generalized.svg)

The relevance of the original definition is as follows:

![define relevance](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance.svg)

![define relevance 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance_2.svg)

![pdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/pdf.svg)  
![logpdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/logpdf.svg)  
![cdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/cdf.svg)  

## Statistics

|stat|x|note|
|----|----|----|
|mean|N/A|undefined|
|variance|N/A|undefined|
|mode|-0.22278298125640850406...|p(x)=1.806556338205509427830338852686311455672580...e-1|
|0.01-quantile|-2.10489790934939769338...||
|0.05-quantile|-1.49825415177780273396...||
|0.1-quantile|-1.09225452805484635423...||
|0.25-quantile|-0.20464065154575316905...||
|0.5-quantile|1.35578042099080132503...||
|0.75-quantile|4.45839461019464834851...||
|0.9-quantile|11.649284684474405569996...||
|0.95-quantile|22.450278078872781782888...|
|0.99-quantile|104.156361812207433543595...||
|moment-generating func.|--|undefined|

## Property of Tail

The right side is a fat-tail.

![tail largex](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex.svg)  
![tail largex cdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex_cdf.svg)  
![tail largex approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex_approx.svg)  

The left side decays rapidly.

![tail lessx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx.svg)  
![tail lessx approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx_approx.svg)

### Reference
[W.Börsch-Supan(1961)](https://nvlpubs.nist.gov/nistpubs/jres/65B/jresv65Bn4p245_A1b.pdf)

## PDF derivative

If the differential value is calculated, the accuracy will be improved when interpolating.

[numeric table](https://github.com/tk-yoshimura/LandauDistribution/tree/main/results/diff_pdf.csv)  

![formula diff png](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/formula_diff_pdf.svg)  
![pdf derivative](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/pdf_derivative.svg)  

## Columns

[Numeric Table](https://github.com/tk-yoshimura/LandauDistribution/tree/main/results/table.csv)  
[Wolfram Alpha Reference Values](https://github.com/tk-yoshimura/LandauDistribution/tree/main/WolframAlphaReference)  
[Numeric Integration](https://github.com/tk-yoshimura/LandauDistribution/tree/main/NumericIntegration)  
[Asymptotic Expansion](https://github.com/tk-yoshimura/LandauDistribution/tree/main/AsymptoticExpansion)  
[Quantile Approximation](https://github.com/tk-yoshimura/LandauDistribution/tree/main/QuantileApproximation)  