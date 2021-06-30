# LandauDistribution - QuantileApproximation

Approximating the quantile function with double precision accuracy for the purpose of generating random numbers that follow the Landau distribution.

## Coordinate Transformation

First, scale the quantile function for cubic spline interpolation.

![quantile 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/quantile_1.svg)  
![quantile 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/quantile_2.svg)  
![quantile 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/quantile_3.svg)  

## Binary Logit

Define binary logit with the following formula.

![define binary logit](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_binary_logit.svg)  
![graph binary logit](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/graph_binary_logit.svg)  

Next, convert the 128bit unsigned integer to a binary logit value.  
For p &gt; 0.5, do the same for two's complement and make the sign positive.

![convert binary logit](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/convert_binary_logit.svg)  

![convert bits binary logit](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/convert_bits_binary_logit.svg)  

[source](https://github.com/tk-yoshimura/ExRandom/blob/main/ExRandom/Transform/BinaryLogit.cs)