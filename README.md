# LandauDistribution

In probability theory, the Landau distribution is a probability distribution named after Lev Landau.  
Because of the distribution's "fat" tail, the moments of the distribution,  
like mean or variance, are undefined. The distribution is a particular case of stable distribution.  
The stochastic variable is traditionally &lambda;, meaning wavelength. 

## Definition

The original Landau distribution defined by Landau can be evaluated on real numbers as follows:  
![define origin](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_origin.svg)

The Landau distribution, generalized to a stable distribution by introducing position and scale parameters, is as follows:  
![define stabledist](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_stabledist_generalized.svg)

The relevance of the original definition is as follows:  
![define relevance](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance.svg)  
![define relevance 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance_2.svg)

[PDF Precision 64](https://github.com/tk-yoshimura/LandauDistribution/tree/main/results/integrate_pdf_precision64.csv)  
[CDF Precision 64](https://github.com/tk-yoshimura/LandauDistribution/tree/main/results/integrate_cdf_precision64.csv)  
![pdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/pdf.svg)  
![logpdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/logpdf.svg)  
![cdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/cdf.svg)  

## Statistics

|stat|&lambda;|note|
|----|----|----|
|mean|N/A|undefined|
|variance|N/A|undefined|
|mode|-0.22278298125640850406...|p(&lambda;)=1.806556338205509427830338852686311455672580...e-1|
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
![tail largex approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex_approx.svg)

The left side decays rapidly.

![tail lessx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx.svg)  
![tail lessx approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx_approx.svg)

## PDF derivative

If the differential value is calculated, the accuracy will be improved when interpolating.

![formula diff pdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/formula_diff_pdf.svg)  
![pdf derivative](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/pdf_derivative.svg)

## Columns
[Numeric Integration](https://github.com/tk-yoshimura/LandauDistribution/tree/main/NumericIntegration)  
[Asymptotic Expansion](https://github.com/tk-yoshimura/LandauDistribution/tree/main/AsymptoticExpansion)  
[Wolfram Alpha Reference Values](https://github.com/tk-yoshimura/LandauDistribution/tree/main/WolframAlphaReference)  

## Páde Based Approximation of PDF, CDF and Quantile
[Digits100 source](https://github.com/tk-yoshimura/LandauDistribution/tree/main/Digits100)  
[Digits100 dll](https://github.com/tk-yoshimura/LandauDistribution/releases)  

## Reference
[L.Landau, "On the energy loss of fast particles by ionization" (1944)](https://www.semanticscholar.org/paper/On-the-energy-loss-of-fast-particles-by-ionization-Landau/037099731178b3aeebca36a054852e4c4866a1c3)  
[W.Börsch-Supan, "On the Evaluation of the Function &Phi;(&lambda;) for Real Values of &lambda;" (1961)](https://nvlpubs.nist.gov/nistpubs/jres/65B/jresv65Bn4p245_A1b.pdf)  
[K.S.Kölbig and B.Schorr, "Asymptotic expansions for the Landau density and distribution functions" (1983)](https://www.sciencedirect.com/science/article/abs/pii/0010465584900651)  
[K.S.Kölbig, "On the integral from 0 to infinity of exp(-mu x t) x t^(nu-1) x log(t)^m x dt" (1982)](https://inspirehep.net/literature/178407)
